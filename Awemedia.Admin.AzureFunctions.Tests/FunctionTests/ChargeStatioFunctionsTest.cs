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

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class ChargeStationFunctionsTest
    {
        private const string Name = "AwemediaConnection";
        private readonly IBaseService<ChargeStation> _chargeStationBaseService;
        
        private readonly IBaseRepository<ChargeStation> _repository;
        private readonly IBaseRepository<ChargeStation> _chargeStationRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseService<User> _userBaseService;
        private readonly IChargeStationService _chargeStationService;
        private readonly IErrorHandler _errorHandler;
        private static DbContextOptions<AwemediaContext> dbContextOptions { get; set; }
        private static readonly string connectionString = string.Empty;
        private readonly ChargeStationFuntions chargeStationFuntions;

        static ChargeStationFunctionsTest()
        {
            var config = Common.InitConfiguration();
            connectionString = config.GetConnectionString(Name);
            dbContextOptions = new DbContextOptionsBuilder<AwemediaContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public ChargeStationFunctionsTest(IApiAuthorization apiAuthorization)
        {
            var context = new AwemediaContext(dbContextOptions);

            _errorHandler = new ErrorHandler();
            _repository = new BaseRepository<ChargeStation>(context, _errorHandler);
            _chargeStationRepository = new BaseRepository<ChargeStation>(context, _errorHandler);
            
            _chargeStationBaseService = new BaseService<ChargeStation>(_chargeStationRepository);
            _userRepository = new BaseRepository<User>(context, _errorHandler);
            _userBaseService = new BaseService<User>(_userRepository);
            _chargeStationService = new ChargeStationService(_chargeStationBaseService, _userBaseService);
            chargeStationFuntions = new ChargeStationFuntions(apiAuthorization);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsFilteredItems()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("{\"PageNum\":\"1\",\"ItemsPerPage\":\"1\",\"SortBy\":\"ChargeControllerId\",\"Reverse\":\"false\",\"SearchText\":\"test\"}", Encoding.UTF8, "application/json");
            var okResult = chargeStationFuntions.GetFiltered(httpRequestMessage, _chargeStationService, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }
    }
}
