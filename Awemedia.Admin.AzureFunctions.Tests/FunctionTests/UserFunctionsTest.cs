using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
using Moq;
using OidcApiAuthorization.Abstractions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class UserFunctionsTest
    {
        private readonly IErrorHandler _errorHandler;
        private readonly Mock<IUserService> _userService;
        private readonly UserFunctions _userFunctions;
        private readonly Mock<IRoleService> _roleService;
        private readonly HttpRequestMessage _httpRequestMessage;

        public UserFunctionsTest()
        {
            _errorHandler = new ErrorHandler();
            _userService = new Mock<IUserService>();
            _roleService = new Mock<IRoleService>();

            var auth = new Mock<IApiAuthorization>();
            auth
                .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult()));
            _userFunctions = new UserFunctions(auth.Object);
            _httpRequestMessage = Common.CreateRequest();
        }

        [Fact]
        public void GetUsers_Test()
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class

            var okResult = _userFunctions.Get(_httpRequestMessage, _userService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void GetUserById_Test()
        {
            var okResult = _userFunctions.GetById(_httpRequestMessage, _userService.Object, _errorHandler, 1);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void AddUser_Test()
        {
            var user = GetUserModel();
            _httpRequestMessage.Content = new StringContent(user, Encoding.UTF8, "application/json");
            var okResult = _userFunctions.Post(_httpRequestMessage, _userService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void UpdateUser_Test()
        {
            var user = GetUserModel();
            _httpRequestMessage.Content = new StringContent(user, Encoding.UTF8, "application/json");
            var okResult = _userFunctions.Put(_httpRequestMessage, _userService.Object, _errorHandler, 1);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void GetRoles_Test()
        {
            var userFunctions = SetClaims();
            var okResult = userFunctions.GetRoles(_httpRequestMessage, _roleService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void GetRoleById_Test()
        {
            var userFunctions = SetClaims();
            var okResult = userFunctions.GetRoleById(_httpRequestMessage, _roleService.Object, _errorHandler, 1);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        private static UserFunctions SetClaims()
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim("extension_UserRoles", "owner"));
            var claims = new ClaimsPrincipal(ci);

            var auth = new Mock<IApiAuthorization>();
            auth
                .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult(claims)));
            var userFunctions = new UserFunctions(auth.Object);
            return userFunctions;
        }

        private string GetUserModel()
        {
            var user = new UserModel
            {
                Id = 1,
                AssignedMerchantsName = "Test",
                City = "Test",
                Email = "Test",
                Country = new Country() { CountryId = 1, CountryName = "usa", Currency = "$" },
                CountryId = 1,
                CreatedDate = DateTime.Now,
                IsActive = true,
                Mobile = "9753115379",
                ModifiedDate = DateTime.Now,
                Name = "TestUser",
                PostalCode = 12345,
                RoleId = 1,
                RoleName = "Test",
                State = "CA"
            };
            var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            return stringContent;
        }

        [InlineData(1, 2, 3)]
        [InlineData(1, 4, 5)]
        [InlineData(1, 62, 63)]
        [Theory]
        public void Test2(int a, int b, int expected)
        {
            var actual = a + b;
            Assert.Equal(expected, actual);
        }
    }
}