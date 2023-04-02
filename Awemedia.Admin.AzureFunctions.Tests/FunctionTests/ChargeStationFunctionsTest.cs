using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
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
    public class ChargeStationFunctionsTest
    {
        private readonly IErrorHandler _errorHandler;
        private readonly Mock<IChargeStationService> _chargeStationService;
        private readonly Mock<IBranchService> _branchService;
        private readonly HttpRequestMessage _httpRequestMessage;

        public ChargeStationFunctionsTest()
        {
            _errorHandler = new ErrorHandler();
            _chargeStationService = new Mock<IChargeStationService>();
            _branchService = new Mock<IBranchService>();

            _httpRequestMessage = Common.CreateRequest();
        }
        private static IEnumerable GetMerchantsTestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetChargStations_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetChargStations_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }
        
        [Test, TestCaseSource(nameof(GetMerchantsTestData))]
        public void Get_WhenCalled_ReturnsFilteredItems(bool auth, string expected)
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class

            var chargStationFunctions = Common.SetAuth<ChargeStationFuntions>(auth);
            var result = chargStationFunctions.GetFiltered(_httpRequestMessage, _chargeStationService.Object, _errorHandler);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }
        private static IEnumerable AddChargeStationTestData()
        {
            yield return new TestCaseData(true, true, false, "OK").SetName("AddChargeStations_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, false, false, "BadRequest").SetName("AddChargeStations_WhenAuthorized_InvalidData_ReturnsBadRequestResult");
            yield return new TestCaseData(true, true, true, "OK").SetName("AddChargeStations_WhenAuthorized_Duplicate_ReturnsOkResult");
            yield return new TestCaseData(false, false, false, "Unauthorized").SetName("AddChargeStations_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }
       
        [Test, TestCaseSource(nameof(AddChargeStationTestData))]
        public void AddChargeStation(bool auth, bool isValid, bool isDuplicate, string expected)
        {
            var stationContent = GetChargeStationModel(isValid);
            _httpRequestMessage.Content = stationContent;

            _chargeStationService
                 .Setup(u => u.IsChargeStationExists(It.IsAny<Guid>()))
                 .Returns<object>((a) => { if (isDuplicate) { return Guid.NewGuid(); } else { return DBNull.Value; } });

            var stationFunctions = Common.SetAuth<ChargeStationFuntions>(auth);
            var result = stationFunctions.Post(_httpRequestMessage, _chargeStationService.Object, _errorHandler);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }
        private static IEnumerable UpdateChargeStationTestData()
        {
            yield return new TestCaseData(true, true, false, "OK").SetName("UpdateChargeStations_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, false, false, "OK").SetName("UpdateChargeStations_WhenAuthorized_InvalidData_ReturnsOkResult");
            yield return new TestCaseData(true, false, true, "OK").SetName("UpdateChargeStations_WhenAuthorized_Duplicate_ReturnsOkResult");
            yield return new TestCaseData(false, false, false, "Unauthorized").SetName("UpdateChargeStations_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }
      
        [Test, TestCaseSource(nameof(UpdateChargeStationTestData))]
        public void UpdateChargeStation(bool auth, bool isValid, bool isDuplicate, string expected)
        {
            var stationContent = GetChargeStationModel(isValid);
            _httpRequestMessage.Content = stationContent;

            _chargeStationService
                .Setup(u => u.IsChargeStationExists(It.IsAny<Guid>()))
                .Returns<object>((a) => { if (isDuplicate) { return Guid.NewGuid(); } else { return DBNull.Value; } });
            _chargeStationService.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(new Business.Models.ChargeStation() { Id = Guid.NewGuid().ToString() });

            var stationFunctions = Common.SetAuth<ChargeStationFuntions>(auth);
            var result = stationFunctions.Put(_httpRequestMessage, _chargeStationService.Object, _errorHandler, Guid.NewGuid().ToString(), _branchService.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private StringContent GetChargeStationModel(bool isValid)
        {
            Business.Models.ChargeStation station = null;
            if (isValid)
            {
                station = new Business.Models.ChargeStation()
                {
                    Id = Guid.NewGuid().ToString(),
                    Geolocation = "3.1874348,101.6357984",
                    ChargeControllerId = "testid",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    DeviceId = "1ca2cc90c68c998a",
                    DeviceToken = "firebase-token",
                    Uid = 470,
                    BranchId = 92,
                    IsActive = true,
                    BatteryLevel = "51%",
                    IsOnline = true,
                    LastPingTimeStamp = Convert.ToDateTime("2021-02-24 05:32:52.777"),
                    AppVersion = "2.0.4 (342)",
                    BatteryInfoDisplayField = "",
                    BranchName = "Happy Tunes - KL",
                    LastBatteryInfoDisplayField = "2020-12-28 07:46:45.673",
                    MerchantName = "Happy Tunes"
                };
            }
            else
            {
                station = new Business.Models.ChargeStation() { };
            }
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(station);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}