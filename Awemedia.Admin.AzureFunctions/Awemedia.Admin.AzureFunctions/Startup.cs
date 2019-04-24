using System.IO;
using AutoMapper;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Awemedia.Chargestation.Api;
using Awemedia.Chargestation.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;


[assembly: WebJobsStartup(typeof(Startup))]
namespace Awemedia.Chargestation.Api
{

    public class Startup : IWebJobsStartup
    {
        private IConfigurationRoot Configuration { get; }
        public Startup()
        {

        }

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            // NLog

            LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }
        private void ConfigureServices(IServiceCollection services)
        {

            services.AddAutoMapper();
            services.AddDbContext<AwemediaContext>(options =>
                {
                    options.UseSqlServer("Server=localhost;Database=Awemedia;user=sa;password=login@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                });
            // Disabling the default behaviour for model validation for 400 response
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            Installer.ConfigureServices(services);
        }

        public void Configure(IWebJobsBuilder builder)
        {
            ConfigureServices(builder.Services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {           
            app.UseMvc();
        }
    }
}
