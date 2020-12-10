using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.DAL
{
    public class AwemediaContextFactory : IDesignTimeDbContextFactory<AwemediaContext>
    {
        public AwemediaContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            Console.WriteLine(configuration["admin-api-sql-migration-connection-string"]);
            var optionsBuilder = new DbContextOptionsBuilder<AwemediaContext>();
            optionsBuilder.UseSqlServer(configuration["admin-api-sql-migration-connection-string"], x => x.MigrationsAssembly("Awemedia.Admin.AzureFunctions.DAL"));

            return new AwemediaContext(optionsBuilder.Options);
        }
        public AwemediaContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();
            Console.WriteLine(configuration["admin-api-sql-migration-connection-string"]);
            var optionsBuilder = new DbContextOptionsBuilder<AwemediaContext>();
            optionsBuilder.UseSqlServer(configuration["admin-api-sql-migration-connection-string"], x => x.MigrationsAssembly("Awemedia.Admin.AzureFunctions.DAL"));

            return new AwemediaContext(optionsBuilder.Options);
        }
    }
}
