using AutoMapper;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Repositories;
using Awemedia.Admin.AzureFunctions.Business.Services;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Xunit;
using Awemedia.Admin.AzureFunctions;
using System.Text;
using System.Net.Http.Headers;
using Awemedia.Admin.AzureFunctions.Business;
using Awemedia.chargestation.API.tests.Common;
using Awemedia.Admin.AzureFunctions.Functions;
using OidcApiAuthorization.Abstractions;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.Graph;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class ChargeStationFunctionsTest
    {
        private readonly Mock<IChargeStationService> _chargeStationService;
        private readonly IErrorHandler _errorHandler;
        private readonly HttpRequestMessage _httpRequestMessage;

        public ChargeStationFunctionsTest()
        {
            _errorHandler = new ErrorHandler();
            _chargeStationService = new Mock<IChargeStationService>();
            _httpRequestMessage = Common.CreateRequest();
        }

        [InlineData(true, "OK")]
        [InlineData(false, "Unauthorized")]
        [Theory]
        public void GetChargeStations_Tests(bool auth, string expected)
        {
            var request = Common.CreateRequest();
            request.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class

            var chargStationFunctions = SetAuth(auth);
            var result = chargStationFunctions.GetFiltered(request, _chargeStationService.Object, _errorHandler);
            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        [InlineData(true, true, false, "OK")]        
        [InlineData(true, false, false, "BadRequest")]
        [InlineData(true, true, true, "OK")]//Todo: This case should be corrected. and return conflict.
        [InlineData(false, false, false, "Unauthorized")]
        [Theory]
        public void AddChargeStation_Tests(bool auth, bool isValid, bool isDuplicate, string expected)
        {
            var stationContent = GetUserModel(isValid);
            _httpRequestMessage.Content = stationContent;

            if (isDuplicate)
            {
                _chargeStationService.Setup(u => u.IsChargeStationExists(It.IsAny<Guid>())).Returns(Guid.NewGuid());
            }
            else
            {
                _chargeStationService.Setup(u => u.IsChargeStationExists(It.IsAny<Guid>())).Returns(DBNull.Value);
            }
            var stationFunctions = SetAuth(auth);
            var result = stationFunctions.Post(_httpRequestMessage, _chargeStationService.Object, _errorHandler);

            Assert.NotNull(result);
            Assert.Equal(expected, result.StatusCode.ToString());
        }

        private static ChargeStationFuntions SetAuth(bool success = true, string claimValue = "owner")
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
                return new ChargeStationFuntions(auth.Object);
            }
            auth
                .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult("BadLogin")));
            return new ChargeStationFuntions(auth.Object);
        }

        private StringContent GetUserModel(bool isValid)
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
