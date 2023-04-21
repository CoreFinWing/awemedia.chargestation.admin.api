using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

        private static IEnumerable GetMerchantsTestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetMerchants_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetMerchants_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetMerchantsTestData))]
        public void GetMerchants(bool auth, string expected)
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class
            var _merchantFunctions = Common.SetAuth<MerchantFunctions>(auth);
            var result = _merchantFunctions.Get(_httpRequestMessage, _merchantService.Object, _errorHandler);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable GetAllNamesTestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetMerchantNames_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetMerchantNames_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetAllNamesTestData))]
        public void GetMerchantNames(bool auth, string expected)
        {
            var _merchantFunctions = Common.SetAuth<MerchantFunctions>(auth);
            var result = _merchantFunctions.GetAllNames(_httpRequestMessage, _merchantService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable GetMerchantByIdTestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetMerchantById_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetMerchantById_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetMerchantByIdTestData))]
        public void GetMerchantById(bool auth, string expected)
        {
            var _merchantFunctions = Common.SetAuth<MerchantFunctions>(auth);
            var result = _merchantFunctions.GetById(_httpRequestMessage, _merchantService.Object, _errorHandler, 1);

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable AddMerchantTestData()
        {
            yield return new TestCaseData(true, true, "OK").SetName("AddMerchant_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, false, "BadRequest").SetName("AddMerchant_WhenAuthorized_InvalidData_ReturnsBadRequestResult");
            yield return new TestCaseData(false, false, "Unauthorized").SetName("AddMerchant_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(AddMerchantTestData))]
        public void AddMerchant(bool auth, bool isValid, string expected)
        {
            var merchant = GetMerchantWithBrnaches(isValid);
            _httpRequestMessage.Content = merchant;

            var _merchantFunctions = Common.SetAuth<MerchantFunctions>(auth);
            var result = _merchantFunctions.Post(_httpRequestMessage, _merchantService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable ToggleMerchantActiveStateTestData()
        {
            yield return new TestCaseData(true, 1, "OK").SetName("ActiveMerchant_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, 0, "BadRequest").SetName("ActiveMerchant_WhenAuthorized_MerchantNotExists_ReturnsBadRequestResult");
            yield return new TestCaseData(false, 1, "Unauthorized").SetName("InActiveMerchant_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(ToggleMerchantActiveStateTestData))]
        public void ActiveMerchant(bool auth, int merchantCount, string expected)
        {
            var model = new object[] { };
            if (merchantCount == 1)
            {
                model = new[] { new { Id = "1", IsActive = "true" } };
            }
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            _httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var _merchantFunctions = Common.SetAuth<MerchantFunctions>(auth);
            var result = _merchantFunctions.Patch(_httpRequestMessage, _merchantService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable UpdateMerchantTestData()
        {
            yield return new TestCaseData(true, 1, "OK").SetName("UpdateMerchant_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, 0, "BadRequest").SetName("UpdateMerchant_WhenAuthorized_MerchantNotExists_ReturnsBadRequestResult");
            yield return new TestCaseData(false, 1, "Unauthorized").SetName("UpdateMerchant_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(UpdateMerchantTestData))]
        public void UpdateMerchant(bool auth, int id, string expected)
        {
            var merchant = GetMerchantWithBrnaches();
            _httpRequestMessage.Content = merchant;
            var _merchantFunctions = Common.SetAuth<MerchantFunctions>(auth);
            var result = _merchantFunctions.Put(_httpRequestMessage, _merchantService.Object, _errorHandler, id);

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable SearchMerchantTestData()
        {
            yield return new TestCaseData(true, true, "OK").SetName("SearchMerchant_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, false, "BadRequest").SetName("SearchMerchant_WhenAuthorized_WithoutQuery_ReturnsBadRequestResult");
            yield return new TestCaseData(false, true, "Unauthorized").SetName("SearchMerchant_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(SearchMerchantTestData))]
        public void SearchMerchant(bool auth, bool withQuery, string expected)
        {
            string search = withQuery ? "?keyword=test" : "";
            _httpRequestMessage.RequestUri = new Uri($"http://localhost/test{search}");

            var _merchantFunctions = Common.SetAuth<MerchantFunctions>(auth);
            var result = _merchantFunctions.Search(_httpRequestMessage, _merchantService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
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