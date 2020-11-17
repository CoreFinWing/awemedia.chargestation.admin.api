using System;
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
           

            LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AwemediaContext>(options =>
                {
                    options.UseSqlServer(Environment.GetEnvironmentVariable("db_connection_string"));
                });
           
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
           
        }

        public void Configure(IWebJobsBuilder builder)
        {
            ConfigureServices(builder.Services);
        }
      
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {           
            app.UseMvc();
        }
    }
}
