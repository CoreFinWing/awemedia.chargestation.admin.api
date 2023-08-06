using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.Business.Repositories;
using Awemedia.Admin.AzureFunctions.Business.Services;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class ChargeOptionsFunctionsTest
    {
        private readonly IErrorHandler _errorHandler;
        private readonly Mock<IChargeOptionService> _chargeOptionsService;
        private readonly HttpRequestMessage _httpRequestMessage;

        public ChargeOptionsFunctionsTest()
        {
            _errorHandler = new ErrorHandler();
            _chargeOptionsService = new Mock<IChargeOptionService>();
            _httpRequestMessage = Common.CreateRequest();
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class
            _httpRequestMessage.Headers.CacheControl.MaxAge = new TimeSpan(0, 2, 0);
            var _chargeOptionFunctions = Common.SetAuth<ChargeOptionFunctions>(true);
            var okResult = _chargeOptionFunctions.Get(_httpRequestMessage, _chargeOptionsService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void Post_WhenCalled_InsertNewChargeOption()
        {
            var content = GetChargeOptionModel(true);
            _httpRequestMessage.Content = content;
            var _chargeOptionFunctions = Common.SetAuth<ChargeOptionFunctions>(true);
            var okResult = _chargeOptionFunctions.Post(_httpRequestMessage, _chargeOptionsService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void Delete_WhenCalled_MarkActiveInActiveChargeOption()
        {
            _httpRequestMessage.Content = new StringContent("[{\"Id\":\"4\",\"IsActive\":\"true\"},{\"Id\":\"6\",\"IsActive\":\"false\"}]", Encoding.UTF8, "application/json");
            var _chargeOptionFunctions = Common.SetAuth<ChargeOptionFunctions>(true);
            var okResult = _chargeOptionFunctions.Put(_httpRequestMessage, _chargeOptionsService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        private bool AddDelegate(ChargeOption chargeOption, out bool isDuplicateRecord, int id)
        {
            isDuplicateRecord = true;
            return false;
        }

        [Fact]
        public void IsDuplicateRecord()
        {
            var content = GetChargeOptionModel(true);
            _httpRequestMessage.Content = content;

            bool duplicate = false;
            _chargeOptionsService
                .Setup(c => c.Add(It.IsAny<Business.Models.ChargeOption>(), out duplicate, It.IsAny<int>()))
                .Callback(() => AddDelegate(null, out duplicate, 1))
                .Returns(true);

            var _chargeOptionFunctions = Common.SetAuth<ChargeOptionFunctions>(true);
            var okResult = _chargeOptionFunctions.Post(_httpRequestMessage, _chargeOptionsService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.True(duplicate);
        }

        private StringContent GetChargeOptionModel(bool isValid)
        {
            Business.Models.ChargeOption option;
            if (isValid)
            {
                option = new Business.Models.ChargeOption()
                {
                    Id = 4,
                    IsActive = true,
                    Currency = "RM",
                    ChargeDuration = 150,
                    CountryId = 75,
                    CountryName = "Malaysia",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Price = 500
                };
            }
            else
            {
                option = new Business.Models.ChargeOption() { };
            }
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(option);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}