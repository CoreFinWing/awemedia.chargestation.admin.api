using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
using Moq;
using System;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class UserFunctionsTest
    {
        private readonly IErrorHandler _errorHandler;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IRoleService> _roleService;
        private readonly HttpRequestMessage _httpRequestMessage;

        public UserFunctionsTest()
        {
            _errorHandler = new ErrorHandler();
            _userService = new Mock<IUserService>();
            _roleService = new Mock<IRoleService>();

            _httpRequestMessage = Common.CreateRequest();
        }

        [InlineData(true, "OK")]
        [InlineData(false, "Unauthorized")]
        [Theory]
        public void Get_WhenCalled_ReturnsFilteredItems(bool auth, string expected)
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class

            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.Get(_httpRequestMessage, _userService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, "OK")]
        [InlineData(false, "Unauthorized")]
        [Theory]
        public void Get_WhenCalled_UserById(bool auth, string expected)
        {
            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.GetById(_httpRequestMessage, _userService.Object, _errorHandler, 1);

            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, true, false, "OK")]
        [InlineData(true, false, false, "BadRequest")]
        [InlineData(true, true, true, "Conflict")]
        [InlineData(false, false, false, "Unauthorized")]
        [Theory]
        public void Post_WhenCalled_AddUser(bool auth, bool isValid, bool duplicateUser, string expected)
        {
            var userContent = GetUserModel(isValid);
            _httpRequestMessage.Content = userContent;
            _userService.Setup(u => u.IsUserDuplicate(It.IsAny<UserModel>())).Returns(duplicateUser);
            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.Post(_httpRequestMessage, _userService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, true, 1, "OK")]
        [InlineData(true, false, 1, "BadRequest")]
        [InlineData(true, true, 0, "BadRequest")]
        [InlineData(false, false, 1, "Unauthorized")]
        [Theory]
        public void Put_WhenCalled_UpdateUser(bool auth, bool isValid, int id, string expected)
        {
            var userContent = GetUserModel(isValid);
            _httpRequestMessage.Content = userContent;
            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.Put(_httpRequestMessage, _userService.Object, _errorHandler, id);

            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, "owner", "OK")]
        [InlineData(true, "not-owner", "OK")]
        [InlineData(false, "owner", "Unauthorized")]
        [Theory]
        public void Get_WhenCalled_Roles(bool auth, string claimValue, string expected)
        {
            var userFunctions = Common.SetAuth<UserFunctions>(auth, claimValue);
            var result = userFunctions.GetRoles(_httpRequestMessage, _roleService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, "OK")]
        [InlineData(false, "Unauthorized")]
        [Theory]
        public void Get_WhenCalled_RolesById(bool auth, string expected)
        {
            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.GetRoleById(_httpRequestMessage, _roleService.Object, _errorHandler, 1);

            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        private StringContent GetUserModel(bool isValid = true)
        {
            UserModel user = null;
            if (isValid)
            {
                user = new UserModel
                {
                    Id = 1,
                    AssignedMerchantsName = isValid ? "Test" : null,
                    City = "Test",
                    Email = "Test",
                    Country = new Country() { CountryId = 1, CountryName = "usa", Currency = "$" },
                    CountryId = 1,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Mobile = "9753115379",
                    ModifiedDate = DateTime.Now,
                    Name = isValid ? "TestUser" : null,
                    PostalCode = 12345,
                    RoleId = 1,
                    RoleName = "Test",
                    State = "CA"
                };
            }
            else
            {
                user = new UserModel { };
            }

            var content = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}