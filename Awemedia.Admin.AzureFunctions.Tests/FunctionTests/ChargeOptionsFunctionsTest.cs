using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Repositories;
using Awemedia.Admin.AzureFunctions.Business.Services;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class ChargeOptionsFunctionsTest
    {
        private const string Name = "AwemediaConnection";
        private readonly ChargeOptionsFunction chargeOptionsFunction;
        private readonly IChargeOptionsService chargeOptionsService;
        private readonly IBaseService<ChargeOptions> _baseService;
        private readonly IBaseRepository<ChargeOptions> _repository;
       
        private readonly IErrorHandler _errorHandler;
        private static DbContextOptions<AwemediaContext> dbContextOptions { get; set; }
        private static readonly string connectionString = string.Empty;

        static ChargeOptionsFunctionsTest()
        {
            var config = Common.InitConfiguration();
            connectionString = config.GetConnectionString(Name);
            dbContextOptions = new DbContextOptionsBuilder<AwemediaContext>()
                .UseSqlServer(connectionString)
                .Options;
        }
        public ChargeOptionsFunctionsTest()
        {
            var context = new AwemediaContext(dbContextOptions);
            _errorHandler = new ErrorHandler();
            _repository = new BaseRepository<ChargeOptions>(context, _errorHandler);
            _baseService = new BaseService<ChargeOptions>(_repository);
            chargeOptionsService = new ChargeOptionsService(_baseService);
            chargeOptionsFunction = new ChargeOptionsFunction();
        }
        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Headers.CacheControl.MaxAge = new TimeSpan(0, 2, 0);
            var okResult = chargeOptionsFunction.Get(httpRequestMessage, chargeOptionsService, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void Post_WhenCalled_InsertNewChargeOption()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("{\"Price\":\"11.00\",\"Currency\":\"RM\",\"ChargeDuration\":\"150 Mins\"}", Encoding.UTF8, "application/json");
            var okResult = chargeOptionsFunction.Add(httpRequestMessage, chargeOptionsService, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }
        [Fact]
        public void Delete_WhenCalled_MarkActiveInActiveChargeOption()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("[{\"Id\":\"4\",\"IsActive\":\"true\"},{\"Id\":\"6\",\"IsActive\":\"false\"}]", Encoding.UTF8, "application/json");
            var okResult = chargeOptionsFunction.Active_InActive(httpRequestMessage, chargeOptionsService, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void IsDuplicateRecord()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("{\"Price\":\"11.00\",\"Currency\":\"RM\",\"ChargeDuration\":\"150 Mins\"}", Encoding.UTF8, "application/json");
            var okResult = chargeOptionsFunction.Add(httpRequestMessage, chargeOptionsService, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("Conflict", okResult.StatusCode.ToString());
        }
    }
}
