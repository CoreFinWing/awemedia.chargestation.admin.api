using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Repositories;
using Awemedia.Admin.AzureFunctions.Business.Services;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class MerchantFunctionsTest
    {
        private const string Name = "AwemediaConnection";
        private readonly IBaseService<Merchant> _merchantBaseService;
        private readonly IBaseRepository<Merchant> _repository;
        private readonly IBaseService<Branch> _branchBaseService;
        private readonly IBaseRepository<Branch> _branchRepository;
        private readonly IMerchantService _merchantService;
        private readonly IBranchService _branchService;
        private readonly IErrorHandler _errorHandler;
        private static DbContextOptions<AwemediaContext> dbContextOptions { get; set; }
        private static readonly string connectionString = string.Empty;
        private readonly MerchantFunctions merchantFunctions;

        static MerchantFunctionsTest()
        {
            var config = Common.InitConfiguration();
            connectionString = config.GetConnectionString(Name);
            dbContextOptions = new DbContextOptionsBuilder<AwemediaContext>()
        .UseSqlServer(connectionString)
        .Options;
        }

        public MerchantFunctionsTest()
        {
            var context = new AwemediaContext(dbContextOptions);

            _errorHandler = new ErrorHandler();
            _branchRepository = new BaseRepository<Branch>(context, _errorHandler);
            _branchBaseService = new BaseService<Branch>(_branchRepository);
            _branchService = new BranchService(_branchBaseService);
            _repository = new BaseRepository<Merchant>(context, _errorHandler);
            _merchantBaseService = new BaseService<Merchant>(_repository);
            _merchantService = new MerchantService(_merchantBaseService, _branchService);
            merchantFunctions = new MerchantFunctions();
        }
        [Fact]
        public void Post_WhenCalled_InsertNewMerchant()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("{\"RegisteredBusinessName\": \"test name 12\",\"LicenseNumber\": \"test lincense nu3\",\"Dba\": \"test DBA\",\"ContactName\": \"test contact\",\"PhoneNumber\": \"test phone\",\"Email\": \"test email\",\"ProfitSharePercentage\": \"test profit\",\"ChargeStationsOrdered\": \"3\",\"DepositMoneyPaid\": \"1000\",\"IndustryTypeId\": 1,\"SecondaryContact\": \"test sec contact\",\"SecondaryPhone\": \"test phone\",\"IndustryName\": \"ktv\",\"IndustryType\": null,\"Branch\": [{\"Name\": \"test test\",\"Address\": \"test test\",\"ContactName\": \"test 1\",\"PhoneNum\": \"test 1\",\"Email\": \"test@gmail.com\",\"MerchantId\": 3,\"Geolocation\": \"674999886298\",\"Merchant\": null,\"CreatedDate\": \"2019-05-31 07:42:57\",\"ModifiedDate\": \"2019-05-31 07:42:57\"},{\"Name\": \"test 2\",\"Address\": \"test 2\",\"ContactName\": \"test 2\",\"PhoneNum\": \"test 2\",\"Email\": \"test2@gmail.com\",\"MerchantId\": 3,\"Geolocation\": \"674999886298\",\"Merchant\": null,\"CreatedDate\": \"2019-05-31 07:43:11\",\"ModifiedDate\": \"2019-05-31 07:43:11\"}],\"CreatedDate\": \"2019-05-31 07:42:23\",\"ModifiedDate\": \"2019-05-31 07:42:23\"}", Encoding.UTF8, "application/json");
            var okResult = merchantFunctions.Post(httpRequestMessage, _merchantService, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }
        [Fact]
        public void Put_WhenCalled_UpdateMerchant()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("{\"Id\": \"6\",\"RegisteredBusinessName\": \"test name 12\",\"LicenseNumber\": \"test lincense nu3\",\"Dba\": \"test DBA\",\"ContactName\": \"test contact\",\"PhoneNumber\": \"test phone\",\"Email\": \"test email\",\"ProfitSharePercentage\": \"test profit\",\"ChargeStationsOrdered\": \"3\",\"DepositMoneyPaid\": \"1000\",\"IndustryTypeId\": 1,\"SecondaryContact\": \"test sec contact\",\"SecondaryPhone\": \"test phone\",\"IndustryName\": \"ktv\",\"IndustryType\": null,\"Branch\": [{\"Name\": \"test test\",\"Address\": \"test test\",\"ContactName\": \"test 1\",\"PhoneNum\": \"test 1\",\"Email\": \"test@gmail.com\",\"MerchantId\": 3,\"Geolocation\": \"674999886298\",\"Merchant\": null,\"CreatedDate\": \"2019-05-31 07:42:57\",\"ModifiedDate\": \"2019-05-31 07:42:57\"},{\"Name\": \"test 2\",\"Address\": \"test 2\",\"ContactName\": \"test 2\",\"PhoneNum\": \"test 2\",\"Email\": \"test2@gmail.com\",\"MerchantId\": 3,\"Geolocation\": \"674999886298\",\"Merchant\": null,\"CreatedDate\": \"2019-05-31 07:43:11\",\"ModifiedDate\": \"2019-05-31 07:43:11\"}],\"CreatedDate\": \"2019-05-31 07:42:23\",\"ModifiedDate\": \"2019-05-31 07:42:23\"}", Encoding.UTF8, "application/json");
            var okResult = merchantFunctions.Post(httpRequestMessage, _merchantService, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }
        [Fact]
        public void Patch_WhenCalled_SoftDeleteMerchant()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("[{\"Id\":\"8\",\"IsActive\":\"false\"}]", Encoding.UTF8, "application/json");
            var okResult = merchantFunctions.Post(httpRequestMessage, _merchantService, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }
    }
}
