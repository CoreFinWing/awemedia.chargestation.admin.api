
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Repositories;
using Awemedia.Admin.AzureFunctions.Business.Services;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Microsoft.Extensions.DependencyInjection;


namespace Awemedia.Chargestation.Api.Infrastructure
{
    internal static class Installer
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<ChargeOptions>, BaseRepository<ChargeOptions>>();
            services.AddTransient<IBaseService<ChargeOptions>, BaseService<ChargeOptions>>();

            services.AddTransient<IBaseRepository<Events>, BaseRepository<Events>>();
            services.AddTransient<IBaseService<Events>, BaseService<Events>>();

            services.AddTransient<IBaseRepository<ChargeStation>, BaseRepository<ChargeStation>>();
            services.AddTransient<IBaseService<ChargeStation>, BaseService<ChargeStation>>();

            services.AddTransient<IChargeStationService, ChargeStationService>();
            services.AddTransient<ILoggerService, LoggerService>();
            services.AddTransient<IErrorHandler, ErrorHandler>();
        }

    }
}
