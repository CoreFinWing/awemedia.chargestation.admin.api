using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Net.Http;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    [TestFixture]
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

        private static IEnumerable GetUsers_TestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetUsers_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetUsers_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetUsers_TestData))]
        public void GetUsers(bool auth, string expected)
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class

            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.Get(_httpRequestMessage, _userService.Object, _errorHandler);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable GetUserById_TestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetUserById_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetUserById_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetUserById_TestData))]
        public void GetUserById(bool auth, string expected)
        {
            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.GetById(_httpRequestMessage, _userService.Object, _errorHandler, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable CreateUser_TestData()
        {
            yield return new TestCaseData(true, true, false, "OK").SetName("CreateUser_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, false, false, "BadRequest").SetName("CreateUser_WhenAuthorized_InvalidData_ReturnsBadRequestResult");
            yield return new TestCaseData(true, true, true, "Conflict").SetName("CreateUser_WhenAuthorized_DuplicateUser_ReturnsConflictResult");
            yield return new TestCaseData(false, false, false, "Unauthorized").SetName("CreateUser_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(CreateUser_TestData))]
        public void CreateUser(bool auth, bool isValid, bool duplicateUser, string expected)
        {
            var userContent = GetUserModel(isValid);
            _httpRequestMessage.Content = userContent;
            _userService.Setup(u => u.IsUserDuplicate(It.IsAny<UserModel>())).Returns(duplicateUser);
            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.Post(_httpRequestMessage, _userService.Object, _errorHandler);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable UpdateUser_TestData()
        {
            yield return new TestCaseData(true, true, 1, "OK").SetName("UpdateUser_WhenAuthorized_ReturnsOk");
            yield return new TestCaseData(true, false, 1, "BadRequest").SetName("UpdateUser_WhenAuthorized_InvalidData_ReturnsBadRequestResult");
            yield return new TestCaseData(true, true, 0, "BadRequest").SetName("UpdateUser_WhenAuthorized_UserNotExists_ReturnsBadRequestResult");
            yield return new TestCaseData(false, false, 1, "Unauthorized").SetName("UpdateUser_WhenNotAuthorized_InvalidData_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(UpdateUser_TestData))]
        public void UpdateUser(bool auth, bool isValid, int id, string expected)
        {
            var userContent = GetUserModel(isValid);
            _httpRequestMessage.Content = userContent;
            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.Put(_httpRequestMessage, _userService.Object, _errorHandler, id);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable GetRoles_TestData()
        {
            yield return new TestCaseData(true, "not-owner", "OK").SetName("GetRoles_WhenAuthorizedWithoutOwnerClaim_ReturnsOkResponseResult");
            yield return new TestCaseData(true, "owner", "OK").SetName("GetRoles_WhenAuthorizedWithOwnerClaim_ReturnsOkResult");
            yield return new TestCaseData(false, "owner", "Unauthorized").SetName("GetRoles_WhenNotAuthorizedWithOwnerClaim_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetRoles_TestData))]
        public void GetRoles(bool auth, string claimValue, string expected)
        {
            var userFunctions = Common.SetAuth<UserFunctions>(auth, claimValue);
            var result = userFunctions.GetRoles(_httpRequestMessage, _roleService.Object, _errorHandler);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable GetRolesById_TestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetRolesById_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetRolesById_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetRolesById_TestData))]
        public void GetRolesById(bool auth, string expected)
        {
            var userFunctions = Common.SetAuth<UserFunctions>(auth);
            var result = userFunctions.GetRoleById(_httpRequestMessage, _roleService.Object, _errorHandler, 1);

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
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