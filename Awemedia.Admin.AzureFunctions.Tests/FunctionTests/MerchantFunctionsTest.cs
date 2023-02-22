using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
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
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
        private readonly Mock<IMerchantService> _merchantService;
        private readonly IBranchService _branchService;
        private readonly IErrorHandler _errorHandler;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseService<User> _userBaseService;
        private static DbContextOptions<AwemediaContext> dbContextOptions { get; set; }
        private static readonly string connectionString = string.Empty;
        private readonly MerchantFunctions merchantFunctions;

        static MerchantFunctionsTest()
        {
            var config = Common.InitConfiguration();
            connectionString = config.GetConnectionString(Name);
            dbContextOptions = new DbContextOptionsBuilder<AwemediaContext>().UseSqlServer(connectionString).Options;
        }

        public MerchantFunctionsTest()
        {
            var context = new AwemediaContext(dbContextOptions);
            _errorHandler = new ErrorHandler();
            _branchRepository = new BaseRepository<Branch>(context, _errorHandler);
            _branchBaseService = new BaseService<Branch>(_branchRepository);
            _userRepository = new BaseRepository<User>(context, _errorHandler);
            _userBaseService = new BaseService<User>(_userRepository);
            _repository = new BaseRepository<Merchant>(context, _errorHandler);
            _merchantBaseService = new BaseService<Merchant>(_repository);
            //_merchantService = new MerchantService(_merchantBaseService, _userBaseService);
            _merchantService = new Mock<IMerchantService>();
            _merchantService.Setup(m => m.AddMerchant(It.IsAny<Business.Models.Merchant>(), 1)).Returns(1);
            _branchService = new BranchService(_branchBaseService, _merchantService.Object, _userBaseService);
            var auth = new Mock<IApiAuthorization>();
            auth
                .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult()));
            merchantFunctions = new MerchantFunctions(auth.Object);
        }

        [Fact]
        public void Post_WhenCalled_InsertNewMerchant()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();

            /*
             "{\"RegisteredBusinessName\": \"test name 12\",\"LicenseNumber\": \"test lincense nu3\",\"Dba\": \"test DBA\",\"ContactName\": \"test contact\",\"PhoneNumber\": \"test phone\",\"Email\": \"test email\",\"ProfitSharePercentage\": \"test profit\",\"ChargeStationsOrdered\": \"3\",\"DepositMoneyPaid\": \"1000\",\"IndustryTypeId\": 1,\"SecondaryContact\": \"test sec contact\",\"SecondaryPhone\": \"test phone\",\"IndustryName\": \"ktv\",\"IndustryType\": null,\"Branch\": [{\"Name\": \"test test\",\"Address\": \"test test\",\"ContactName\": \"test 1\",\"PhoneNum\": \"test 1\",\"Email\": \"test@gmail.com\",\"MerchantId\": 3,\"Geolocation\": \"674999886298\",\"Merchant\": null,\"CreatedDate\": \"2019-05-31 07:42:57\",\"ModifiedDate\": \"2019-05-31 07:42:57\"},{\"Name\": \"test 2\",\"Address\": \"test 2\",\"ContactName\": \"test 2\",\"PhoneNum\": \"test 2\",\"Email\": \"test2@gmail.com\",\"MerchantId\": 3,\"Geolocation\": \"674999886298\",\"Merchant\": null,\"CreatedDate\": \"2019-05-31 07:43:11\",\"ModifiedDate\": \"2019-05-31 07:43:11\"}],\"CreatedDate\": \"2019-05-31 07:42:23\",\"ModifiedDate\": \"2019-05-31 07:42:23\"}"
             */
            var merchant = GetMerchantWithBrnaches();
            httpRequestMessage.Content = new StringContent(merchant, Encoding.UTF8, "application/json");

            var okResult = merchantFunctions.Post(httpRequestMessage, _merchantService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void Put_WhenCalled_UpdateMerchant()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            var merchant = GetMerchantWithBrnaches();
            httpRequestMessage.Content = new StringContent(merchant, Encoding.UTF8, "application/json");
            var okResult = merchantFunctions.Post(httpRequestMessage, _merchantService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
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

        private string GetMerchantWithBrnaches()
        {
            /* "\"Id\": \"6\",\"RegisteredBusinessName\": \"test name 12\",\"LicenseNumber\": \"test lincense nu3\",\"Dba\": \"test DBA\",\"ContactName\": \"test contact\",\"PhoneNumber\": \"test phone\",\"Email\": \"test email\",\"ProfitSharePercentage\": \"test profit\",\"ChargeStationsOrdered\": \"3\",\"DepositMoneyPaid\": \"1000\",\"IndustryTypeId\": 1,\"SecondaryContact\": \"test sec contact\",\"SecondaryPhone\": \"test phone\",\"IndustryName\": \"ktv\",\"IndustryType\": null,\"Branch\": [{\"Name\": \"test test\",\"Address\": \"test test\",\"ContactName\": \"test 1\",\"PhoneNum\": \"test 1\",\"Email\": \"test@gmail.com\",\"MerchantId\": 3,\"Geolocation\": \"674999886298\",\"Merchant\": null,\"CreatedDate\": \"2019-05-31 07:42:57\",\"ModifiedDate\": \"2019-05-31 07:42:57\"},{\"Name\": \"test 2\",\"Address\": \"test 2\",\"ContactName\": \"test 2\",\"PhoneNum\": \"test 2\",\"Email\": \"test2@gmail.com\",\"MerchantId\": 3,\"Geolocation\": \"674999886298\",\"Merchant\": null,\"CreatedDate\": \"2019-05-31 07:43:11\",\"ModifiedDate\": \"2019-05-31 07:43:11\"}],\"CreatedDate\": \"2019-05-31 07:42:23\",\"ModifiedDate\": \"2019-05-31 07:42:23\"";
            */

            var merchant = new Business.Models.Merchant()
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

            var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(merchant);
            return stringContent;
        }

        [Fact]
        public void Patch_WhenCalled_SoftDeleteMerchant()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();

            var merchant = GetMerchant();

            httpRequestMessage.Content = new StringContent(merchant, Encoding.UTF8, "application/json");
            var okResult = merchantFunctions.Post(httpRequestMessage, _merchantService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        private string GetMerchant()
        {
            var merchant = new Business.Models.Merchant()
            {
                RegisteredBusinessName = "awepay softwares",
                ChargeStationsOrdered = "3",
                IndustryType = new IndustryType() { Id = 1, IsActive = true, Name = "Software" },
                ContactName = "baljeet",
                IndustryTypeId = 1,
                Dba = "test dba",
                DepositMoneyPaid = "1150",
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
                ModifiedDate = DateTime.Now
            };

            var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(merchant);
            return stringContent;
        }
    }
}