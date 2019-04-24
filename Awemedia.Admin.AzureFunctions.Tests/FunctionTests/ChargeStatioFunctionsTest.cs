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
using Awemedia.Chargestation.AzureFunctions.Infrastructure;
using Awemedia.chargestation.API.tests.Common;
using Awemedia.Admin.AzureFunctions.Functions;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class ChargeStationFunctionsTest
    {
        private const string Name = "AwemediaConnection";
        private readonly IBaseService<ChargeStation> _chargeStationBaseService;
        private readonly IBaseRepository<ChargeStation> _repository;
        private readonly IBaseRepository<ChargeStation> _chargeStationRepository;
        private readonly IChargeStationService _chargeStationService;
        private readonly Mapper _mapper;
        private readonly IErrorHandler _errorHandler;
        private static DbContextOptions<AwemediaContext> dbContextOptions { get; set; }
        private static readonly string connectionString = string.Empty;
        private readonly ChargeStationFuntion _chargeStationFuntion;

        static ChargeStationFunctionsTest()
        {
            var config = Common.InitConfiguration();
            connectionString = config.GetConnectionString(Name);
            dbContextOptions = new DbContextOptionsBuilder<AwemediaContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public ChargeStationFunctionsTest()
        {
            var context = new AwemediaContext(dbContextOptions);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile())));
            _errorHandler = new ErrorHandler();
            _repository = new BaseRepository<ChargeStation>(context, _errorHandler);
            _chargeStationRepository = new BaseRepository<ChargeStation>(context, _errorHandler);
            _chargeStationBaseService = new BaseService<ChargeStation>(_chargeStationRepository);
            _chargeStationService = new ChargeStationService(_chargeStationBaseService, _mapper);
            _chargeStationFuntion = new ChargeStationFuntion(_chargeStationService, _mapper, _errorHandler);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsFilteredItems()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("{\"PageNum\":\"1\",\"ItemsPerPage\":\"1\",\"SortBy\":\"ChargeControllerId\",\"Reverse\":\"false\",\"SearchText\":\"test\"}", Encoding.UTF8, "application/json");
            var okResult = _chargeStationFuntion.GetFiltered(httpRequestMessage);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }
    }
}
