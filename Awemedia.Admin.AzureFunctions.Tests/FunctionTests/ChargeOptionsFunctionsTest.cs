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
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Awemedia.Admin.AzureFunctions.Tests.FunctionTests
{
    public class ChargeOptionsFunctionsTest
    {
        private const string Name = "AwemediaConnection";
        private readonly ChargeOptionFunctions chargeOptionFunctions;
        private readonly Mock<IChargeOptionService> chargeOptionsService;
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
            chargeOptionsService = new Mock<IChargeOptionService>();
            // chargeOptionFunctions = new ChargeOptionFunctions(apiAuthorization);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.RequestUri = new Uri("http://localhost/test?Search=test&IsActive=true");//BaseSearchFilter model class
            httpRequestMessage.Headers.CacheControl.MaxAge = new TimeSpan(0, 2, 0);
            var _chargeOptionFunctions = SetAuth(true);
            var okResult = _chargeOptionFunctions.Get(httpRequestMessage, chargeOptionsService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void Post_WhenCalled_InsertNewChargeOption()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            var content = GetChargeOptionModel(true);
            httpRequestMessage.Content = content;
            var _chargeOptionFunctions = SetAuth(true);
            var okResult = _chargeOptionFunctions.Post(httpRequestMessage, chargeOptionsService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }

        [Fact]
        public void Delete_WhenCalled_MarkActiveInActiveChargeOption()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("[{\"Id\":\"4\",\"IsActive\":\"true\"},{\"Id\":\"6\",\"IsActive\":\"false\"}]", Encoding.UTF8, "application/json");
            var _chargeOptionFunctions = SetAuth(true);
            var okResult = _chargeOptionFunctions.Put(httpRequestMessage, chargeOptionsService.Object, _errorHandler);
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
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            
            var content = GetChargeOptionModel(true);
            httpRequestMessage.Content = content;

            bool duplicate = false;
            chargeOptionsService
                .Setup(c => c.Add(It.IsAny<Business.Models.ChargeOption>(), out duplicate, It.IsAny<int>()))
                .Callback(() => AddDelegate(null, out duplicate, 1))
                .Returns(true);
            
            var _chargeOptionFunctions = SetAuth(true);
            var okResult = _chargeOptionFunctions.Post(httpRequestMessage, chargeOptionsService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.True(duplicate);
        }

        private static ChargeOptionFunctions SetAuth(bool success = true, string claimValue = "owner")
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
                return new ChargeOptionFunctions(auth.Object);
            }
            auth
                .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult("BadLogin")));
            return new ChargeOptionFunctions(auth.Object);
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