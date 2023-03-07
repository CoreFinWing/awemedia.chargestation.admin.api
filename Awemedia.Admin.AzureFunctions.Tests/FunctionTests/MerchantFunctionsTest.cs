using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
using Moq;
using OidcApiAuthorization.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class MerchantFunctionsTest
    {
        private readonly IErrorHandler _errorHandler;
        private readonly Mock<IMerchantService> _merchantService;
        private readonly HttpRequestMessage _httpRequestMessage;
        public MerchantFunctionsTest()
        {
            _errorHandler = new ErrorHandler();

            _merchantService = new Mock<IMerchantService>();
            _merchantService.Setup(m => m.AddMerchant(It.IsAny<Business.Models.Merchant>(), 1)).Returns(1);
            _httpRequestMessage = Common.CreateRequest();
        }

        [InlineData(true, "OK")]
        [InlineData(false, "Unauthorized")]
        [Theory]
        public void Get_WhenCalled_ReturnsFilteredItems(bool auth, string expected)
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class
            var _merchantFunctions = SetAuth(auth);
            var result = _merchantFunctions.Get(_httpRequestMessage, _merchantService.Object, _errorHandler);
            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, "OK")]
        [InlineData(false, "Unauthorized")]
        [Theory]
        public void Get_WhenCalled_MerchantNames(bool auth, string expected)
        {            
            var _merchantFunctions = SetAuth(auth);
            var result = _merchantFunctions.GetAllNames(_httpRequestMessage, _merchantService.Object, _errorHandler);
            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, "OK")]
        [InlineData(false, "Unauthorized")]
        [Theory]
        public void Get_WhenCalled_MerchantById(bool auth, string expected)
        {
            var _merchantFunctions = SetAuth(auth);
            var result = _merchantFunctions.GetById(_httpRequestMessage, _merchantService.Object, _errorHandler, 1);
            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, true, "OK")]
        [InlineData(true, false, "BadRequest")]
        [InlineData(false, false, "Unauthorized")]
        [Theory]
        public void Post_WhenCalled_AddMerchant(bool auth, bool isValid, string expected)
        {
            var merchant = GetMerchantWithBrnaches(isValid);
            _httpRequestMessage.Content = merchant;

            var _merchantFunctions = SetAuth(auth);
            var okResult = _merchantFunctions.Post(_httpRequestMessage, _merchantService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal(expected, okResult.StatusCode.ToString());
        }

        [InlineData(true, 1, "OK")]
        [InlineData(true, 0, "BadRequest")]
        [InlineData(false, 1, "Unauthorized")]
        [Theory]
        public void Patch_WhenCalled_Active_InActive_Merchant(bool auth, int merchantCount, string expected)
        {
            var model = new object[] { };
            if (merchantCount == 1)
            {
                model = new[] { new { Id = "1", IsActive = "true" } };
            }
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            _httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var _merchantFunctions = SetAuth(auth);
            var okResult = _merchantFunctions.Patch(_httpRequestMessage, _merchantService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal(expected, okResult.StatusCode.ToString());
        }

        [InlineData(true, 1, "OK")]
        [InlineData(true, 0, "BadRequest")]
        [InlineData(false, 1, "Unauthorized")]
        [Theory]
        public void Put_WhenCalled_UpdateMerchant(bool auth, int id, string expected)
        {
            var merchant = GetMerchantWithBrnaches();
            _httpRequestMessage.Content = merchant;
            var _merchantFunctions = SetAuth(auth);
            var okResult = _merchantFunctions.Put(_httpRequestMessage, _merchantService.Object, _errorHandler, id);
            Assert.NotNull(okResult);
            Assert.Equal(expected, okResult.StatusCode.ToString());
        }

        [InlineData(true, true, "OK")]
        [InlineData(true, false, "BadRequest")]
        [InlineData(false, true, "Unauthorized")]
        [Theory]
        public void Get_WhenCalled_AutoCompleteSearchMerchant(bool auth, bool withQuery, string expected)
        {
            string search = withQuery ? "?keyword=test" : "";
            _httpRequestMessage.RequestUri = new Uri($"http://localhost/test{search}");

            var _merchantFunctions = SetAuth(auth);
            var okResult = _merchantFunctions.Search(_httpRequestMessage, _merchantService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal(expected, okResult.StatusCode.ToString());
        }

        private static MerchantFunctions SetAuth(bool success = true, string claimValue = "owner")
        {
            var auth = new Mock<IApiAuthorization>();
            if (success)
            {
                var ci = new ClaimsIdentity();
                ci.AddClaim(new Claim("extension_UserRoles", claimValue));
                ci.AddClaim(new Claim("emails", "baljeet@awepay.com"));
                var claims = new ClaimsPrincipal(ci);

                auth
                    .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                    .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult(claims)));
                return new MerchantFunctions(auth.Object);
            }
            auth
                .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult("BadLogin")));
            return new MerchantFunctions(auth.Object);
        }

        private List<Business.Models.Branch> GetBranches()
        {
            var branch = new Business.Models.Branch()
            {
                Name = "Branch-one",
                Address = "Au",
                City = "test city",
                ContactName = "",
                CountryId = 1,
                CountryName = "AU",
                CreatedDate = DateTime.Now,
                Email = "baljeet@awepay.com",
                Id = 1,
                Geolocation = "674999886298",
                MerchantId = 1,
                IsActive = true,
                PhoneNum = "9876543211",
                MerchantName = "awepay softwares",
                PostalCode = "12345",
                ModifiedDate = DateTime.Now,
            };

            return new List<Business.Models.Branch>() { branch };
        }

        private StringContent GetMerchantWithBrnaches(bool isValid = true)
        {
            Business.Models.Merchant merchant = null;
            if (isValid)
            {
                merchant = new Business.Models.Merchant()
                {
                    RegisteredBusinessName = "test name 12",
                    ChargeStationsOrdered = "3",
                    IndustryType = new IndustryType() { Id = 1, IsActive = true, Name = "Software" },
                    ContactName = "baljeet",
                    IndustryName = "ktv",
                    IndustryTypeId = 1,
                    Dba = "test DBA",
                    DepositMoneyPaid = "1500",
                    Email = "baljeet@awepay.com",
                    Id = 8,
                    IsActive = false,
                    LicenseNumber = "DL123456",
                    NumOfActiveLocations = 1,
                    PhoneNumber = "9876054313",
                    ProfitSharePercentage = "15",
                    SecondaryContact = "Vishal",
                    SecondaryPhone = "9976054322",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Branch = GetBranches()
                };
            }
            else
            {
                merchant = new Business.Models.Merchant();
            }

            var content = Newtonsoft.Json.JsonConvert.SerializeObject(merchant);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}