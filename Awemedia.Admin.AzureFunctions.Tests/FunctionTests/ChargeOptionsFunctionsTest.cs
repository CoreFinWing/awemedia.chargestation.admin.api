﻿using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
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

        private static IEnumerable GetChargeOptionsTestData()
        {
            yield return new TestCaseData(true, "OK").SetName("GetChargeOptions_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(false, "Unauthorized").SetName("GetChargeOptions_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(GetChargeOptionsTestData))]
        public void GetChargeOptions(bool auth, string expected)
        {
            _httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class
            _httpRequestMessage.Headers.CacheControl.MaxAge = new TimeSpan(0, 2, 0);
            var _chargeOptionFunctions = Common.SetAuth<ChargeOptionFunctions>(auth);
            var result = _chargeOptionFunctions.Get(_httpRequestMessage, _chargeOptionsService.Object, _errorHandler);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusCode.ToString());
        }

        private static IEnumerable AddChargeOptionTestData()
        {
            yield return new TestCaseData(true, true, "OK").SetName("AddChargeOption_WhenAuthorized_ReturnsOkResult");
            yield return new TestCaseData(true, false, "BadRequest").SetName("AddChargeOption_WhenAuthorized_InvalidData_ReturnsBadRequestResult");
            yield return new TestCaseData(false, false, "Unauthorized").SetName("AddChargeOption_WhenNotAuthorized_ReturnsUnauthorizedResult");
        }

        [Test, TestCaseSource(nameof(AddChargeOptionTestData))]
        public void AddChargeOption(bool auth, bool isValid, string expected)
        {
            var content = GetChargeOptionModel(isValid);
            _httpRequestMessage.Content = content;
            var _chargeOptionFunctions = Common.SetAuth<ChargeOptionFunctions>(auth);
            var okResult = _chargeOptionFunctions.Post(_httpRequestMessage, _chargeOptionsService.Object, _errorHandler);

            Assert.NotNull(okResult);
            Assert.AreEqual(expected, okResult.StatusCode.ToString());
        }

        [Test]
        public void Actvie_InActiveChargeOption()
        {
            _httpRequestMessage.Content = new StringContent("[{\"Id\":\"4\",\"IsActive\":\"true\"},{\"Id\":\"6\",\"IsActive\":\"false\"}]", Encoding.UTF8, "application/json");
            var _chargeOptionFunctions = Common.SetAuth<ChargeOptionFunctions>(true);
            var okResult = _chargeOptionFunctions.Put(_httpRequestMessage, _chargeOptionsService.Object, _errorHandler);

            Assert.NotNull(okResult);
            Assert.AreEqual("OK", okResult.StatusCode.ToString());
        }

        private bool AddDelegate(ChargeOption chargeOption, out bool isDuplicateRecord, int id)
        {
            isDuplicateRecord = true;
            return false;
        }

        [Test]
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