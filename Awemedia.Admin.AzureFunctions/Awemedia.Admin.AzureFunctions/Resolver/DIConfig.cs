using Autofac;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure.ErrorHandler;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Repositories;
using Awemedia.Admin.AzureFunctions.Business.Services;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using AzureFunctions.Autofac.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Resolver
{
    public class DIConfig
    {
        public DIConfig(string functionName)
        {
            DependencyInjection.Initialize(builder =>
            {
                builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
                builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>));
                builder.RegisterType<AwemediaContext>();
                builder.RegisterType<BaseRepository<ChargeOptions>>().As<IBaseRepository<ChargeOptions>>();
                builder.RegisterType<BaseService<ChargeOptions>>().As<IBaseService<ChargeOptions>>();

                builder.RegisterType<ErrorHandler>().As<IErrorHandler>();
                builder.RegisterType<ChargeStationService>().As<IChargeStationService>();
                builder.RegisterType<ChargeOptionsService>().As<IChargeOptionService>();
                builder.RegisterType<MerchantService>().As<IMerchantService>();
                builder.RegisterType<BranchService>().As<IBranchService>();
                builder.RegisterType<NotificationService>().As<INotificationService>();
                builder.RegisterType<ChargeSessionService>().As<IChargeSessionService>();
                builder.RegisterType<EmailService>().As<IEmailService>();
                builder.RegisterType<EventService>().As<IEventService>();
                builder.RegisterType<PromotionService>().As<IPromotionService>();
                builder.RegisterType<CountryService>().As<ICountryService>();
                builder.RegisterType<UserService>().As<IUserService>();
            }, functionName);
        }
    }
}
