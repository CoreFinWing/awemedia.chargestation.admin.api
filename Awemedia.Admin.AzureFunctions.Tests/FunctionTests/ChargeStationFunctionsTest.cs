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
        private readonly Mock<INotificationService> _notificationService;
        private readonly Mock<IBranchService> _branchService;
        private readonly HttpRequestMessage _httpRequestMessage;

        public ChargeStationFunctionsTest()
        {
            _errorHandler = new ErrorHandler();
            _chargeStationService = new Mock<IChargeStationService>();
            _notificationService = new Mock<INotificationService>();
            _branchService = new Mock<IBranchService>();

            _httpRequestMessage = Common.CreateRequest();
        }

        private static IEnumerable GetFilteredChargStationsTestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetFilteredChargStations_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetFilteredChargStations_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetFilteredChargStationsTestData))]
        public void GetFilteredChargStations(bool auth, string expected)
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class

            var chargStationFunctions = Common.SetAuth<ChargeStationFuntions>(auth);
            var result = chargStationFunctions.GetFiltered(_httpRequestMessage, _chargeStationService.Object, _errorHandler);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable AddChargeStationTestData()
        {
            yield return new TestCaseData(true, true, false, "OK").SetName("AddChargeStation_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, false, false, "BadRequest").SetName("AddChargeStation_WhenAuthorized_InvalidData_ReturnsBadRequestResult");
            yield return new TestCaseData(true, true, true, "OK").SetName("AddChargeStation_WhenAuthorized_Duplicate_ReturnsOkResult");
            yield return new TestCaseData(false, false, false, "Unauthorized").SetName("AddChargeStation_WhenNotAuthorized_ReturnsUnauthorizedResult");
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
            yield return new TestCaseData(true, true, false, "OK").SetName("UpdateChargeStation_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, false, false, "OK").SetName("UpdateChargeStation_WhenAuthorized_InvalidData_ReturnsOkResult");
            yield return new TestCaseData(true, false, true, "OK").SetName("UpdateChargeStation_WhenAuthorized_Duplicate_ReturnsOkResult");
            yield return new TestCaseData(false, false, false, "Unauthorized").SetName("UpdateChargeStation_WhenNotAuthorized_ReturnsUnauthorizedResult");
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

        private static IEnumerable GetChargeStationTestData()
        {
            yield return new TestCaseData(false, false, true, "Unauthorized").SetName("GetChargeStation_WhenNotAuthorized_ReturnsUnauthorizedResult");
            yield return new TestCaseData(true, false, true, "BadRequest").SetName("GetChargeStation_WhenAuthorized_InvalidData_ReturnsBadRequestResult");
            yield return new TestCaseData(true, true, false, "OK").SetName("GetChargeStation_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, true, true, "BadRequest").SetName("GetChargeStation_WhenAuthorized_ValidationFailed_ReturnsBadRequestResult");
        }

        [Test, TestCaseSource(nameof(GetChargeStationTestData))]
        public void GetChargeStation(bool auth, bool modelInvalid, bool validationFailed, string expected)
        {
            var model = new Business.Models.NotificationPayload()
            {
                Command = modelInvalid ? "Test" : "",
                CommandParams = new System.Collections.Generic.Dictionary<string, string> { { "Duration", validationFailed ? "101" : "99" } }
            };
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var body = new StringContent(content, Encoding.UTF8, "application/json");
            _httpRequestMessage.Content = body;

            Environment.SetEnvironmentVariable("remote_push_max_charge_duration", "100");

            _chargeStationService.Setup(c => c.GetById(It.IsAny<int>())).Returns(new DAL.DataContracts.ChargeStation() { DeviceId = "1" });
            _notificationService.Setup(n => n.SendNotification(It.IsAny<Business.Models.Notification>()));

            var stationFunctions = Common.SetAuth<ChargeStationFuntions>(auth);
            var result = stationFunctions.Get(_httpRequestMessage, _notificationService.Object, _chargeStationService.Object, _errorHandler, "1");

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable GetChargeStationDetailByIdTestData()
        {
            yield return new TestCaseData(false, false, "Unauthorized").SetName("GetChargeStationDetailById_WhenNotAuthorized_ReturnsUnauthorizedResult");
            yield return new TestCaseData(true, false, "OK").SetName("GetChargeStationDetailById_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, true, "BadRequest").SetName("GetChargeStationDetailById_WhenAuthorized_InvalidData_ReturnsBadRequestResult");
        }

        [Test, TestCaseSource(nameof(GetChargeStationDetailByIdTestData))]
        public void GetChargeStationDetailById(bool auth, bool invalid, string expected)
        {
            _chargeStationService.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(new Business.Models.ChargeStation() { });
            var id = invalid ? "" : Guid.NewGuid().ToString();
            var stationFunctions = Common.SetAuth<ChargeStationFuntions>(auth);
            var result = stationFunctions.GetById(_httpRequestMessage, _chargeStationService.Object, _errorHandler, id);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable ToggleChargeStationActiveStateTestData()
        {
            yield return new TestCaseData(true, 1, "OK").SetName("ActiveChargeStation_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, 0, "BadRequest").SetName("ActiveChargeStation_WhenAuthorized_ChargeStationNotExists_ReturnsBadRequestResult");
            yield return new TestCaseData(false, 1, "Unauthorized").SetName("InActiveChargeStation_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(ToggleChargeStationActiveStateTestData))]
        public void ActiveChargeStation(bool auth, int chargeStationCount, string expected)
        {
            var model = new object[] { };
            if (chargeStationCount == 1)
            {
                model = new[] { new { Id = "1", IsActive = "true" } };
            }
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            _httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var stationFunctions = Common.SetAuth<ChargeStationFuntions>(auth);
            var result = stationFunctions.Patch(_httpRequestMessage, _chargeStationService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable ChargeStationDetachFromBranchTestData()
        {
            yield return new TestCaseData(false, false, "Unauthorized").SetName("ChargeStationDetachFromBranch_WhenNotAuthorized_ReturnsUnauthorizedResult");
            yield return new TestCaseData(true, false, "OK").SetName("ChargeStationDetachFromBranch_WhenAuthorized_ChargeStationNotExists_ReturnsBadRequestResult");
            yield return new TestCaseData(true, true, "OK").SetName("ChargeStationDetachFromBranch_WhenAuthorized_ReturnsOkResult");
        }

        [Test, TestCaseSource(nameof(ChargeStationDetachFromBranchTestData))]
        public void ChargeStationDetachFromBranch(bool auth, bool stationExists, string expected)
        {
            var stationFunctions = Common.SetAuth<ChargeStationFuntions>(auth);
            _chargeStationService.Setup(c => c.GetById(It.IsAny<Guid>())).Returns(new Business.Models.ChargeStation() { Id = Guid.NewGuid().ToString() });
            if (stationExists)
            {
                _chargeStationService.Setup(c => c.IsChargeStationExists(It.IsAny<Guid>())).Returns(new Business.Models.ChargeStation() { Id = Guid.NewGuid().ToString() });
            }
            var result = stationFunctions.DetachFromBranch(_httpRequestMessage, _chargeStationService.Object, _errorHandler, Guid.NewGuid().ToString(), _branchService.Object);

            Assert.NotNull(result);
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