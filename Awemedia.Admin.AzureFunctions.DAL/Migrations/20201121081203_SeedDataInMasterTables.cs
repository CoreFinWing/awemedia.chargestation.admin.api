using Awemedia.Admin.AzureFunctions.DAL.Common;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Linq;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class SeedDataInMasterTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = new AwemediaContext())
            {
                foreach (var type in Constants.sessionTypes)
                {
                    if (!context.SessionType.Any(s => s.Type == type.Value))
                    {
                        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.SessionType ON");
                        migrationBuilder.InsertData(table: "SessionType",
                            columns: new[] { "Id", "IsActive", "Type" },
                            values: new object[,] {
                                { type.Key,true,type.Value },
                                 });
                        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.SessionType OFF");
                    }
                }

                foreach (var status in Constants.sessionStatuses)
                {
                    if (!context.SessionStatus.Any(s => s.Status == status.Value))
                    {
                        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.SessionStatus ON");
                        migrationBuilder.InsertData(table: "SessionStatus",
                           columns: new[] { "Id", "Status", "IsActive" },
                           values: new object[,] {
                { status.Key,status.Value,true },
                           });
                        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.SessionStatus OFF");
                    }
                }

                foreach (var type in Constants.industryTypes)
                {
                    if (!context.IndustryType.Any(s => s.Name == type.Value))
                    {

                        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.IndustryType ON");
                        migrationBuilder.InsertData(table: "IndustryType",
                            columns: new[] { "Id", "Name", "IsActive", "CreatedDate", "ModifiedDate" },
                            values: new object[,] {
                { type.Key,type.Value,true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                            });
                        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.IndustryType OFF");
                    }
                }
                foreach (var type in Constants.eventTypes)
                {
                    if (!context.EventType.Any(s => s.Name == type.Value))
                    {
                        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.EventType ON");
                        migrationBuilder.InsertData(table: "EventType",
                           columns: new[] { "Id", "Name", "IsActive", "CreatedDate", "ModifiedDate" },
                           values: new object[,] {
                { type.Key,type.Value,true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                   });
                        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.EventType OFF");
                    }
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
