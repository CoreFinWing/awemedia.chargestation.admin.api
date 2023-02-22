using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Services;
using Awemedia.Admin.AzureFunctions.Functions;
using Awemedia.chargestation.API.tests.Common;
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
    public class UserFunctionsTest
    {
        private readonly IErrorHandler _errorHandler;
        private readonly Mock<IUserService> _userService;
        private readonly UserFunctions _userFunctions;
        public UserFunctionsTest()
        {
            _errorHandler = new ErrorHandler();
            _userService = new Mock<IUserService>();

            var auth = new Mock<IApiAuthorization>();
            auth
                .Setup(s => s.AuthorizeAsync(It.IsAny<HttpRequestHeaders>()))
                .Returns(Task.FromResult(new OidcApiAuthorization.Models.ApiAuthorizationResult()));
            _userFunctions = new UserFunctions(auth.Object);
        }
        [Fact]
        public void Test1()
        {
            HttpRequestMessage httpRequestMessage = Common.CreateRequest();
            httpRequestMessage.Content = new StringContent("", Encoding.UTF8, "application/json");

            var okResult = _userFunctions.Get(httpRequestMessage, _userService.Object, _errorHandler);
            Assert.NotNull(okResult);
            Assert.Equal("OK", okResult.StatusCode.ToString());
        }


    }
}
