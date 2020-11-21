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
                if (!context.SessionType.Any())
                {
                    migrationBuilder.Sql("SET IDENTITY_INSERT dbo.SessionType ON");
                    migrationBuilder.InsertData(table: "SessionType",
                        columns: new[] { "Id", "IsActive", "Type" },
                        values: new object[,] {
                { 1,true,"Promotion" },
                { 2,true,"Paid"  } });
                    migrationBuilder.Sql("SET IDENTITY_INSERT dbo.SessionType OFF");
                }
                if (!context.SessionStatus.Any())
                {
                    migrationBuilder.Sql("SET IDENTITY_INSERT dbo.SessionStatus ON");
                    migrationBuilder.InsertData(table: "SessionStatus",
                       columns: new[] { "Id", "Status", "IsActive" },
                       values: new object[,] {
                { 1,"New",true },
                { 2,"AuthorizationSuccessful",true },
                { 3,"AuthorizationFailure",true },
                { 4,"PaymentRequestSent",true },
                { 5,"PaymentCompleted",true },
                { 6,"PaymentFailure",true },
                { 7,"PaymentTimeout",true },
                { 8,"PaymentCanceledByUser",true },
                { 9,"Charging",true },
                { 10,"ChargingCompleted",true },
                { 11,"ChargeFailed",true },
                { 12,"SessionTimeout",true },
                { 13,"AuthorizationTimeout",true },
                { 14,"PaymentRequestTimeout",true },
                { 15,"ServerTimeOut",true }
                       });
                    migrationBuilder.Sql("SET IDENTITY_INSERT dbo.SessionStatus OFF");
                }

                if (!context.IndustryType.Any())
                {

                    migrationBuilder.Sql("SET IDENTITY_INSERT dbo.IndustryType ON");
                    migrationBuilder.InsertData(table: "IndustryType",
                        columns: new[] { "Id", "Name", "IsActive", "CreatedDate", "ModifiedDate" },
                        values: new object[,] {
                { 1,"F&b (food and beverage)",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 2,"Karaoke",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 3,"Service",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 4,"Event",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },

                        });
                    migrationBuilder.Sql("SET IDENTITY_INSERT dbo.IndustryType OFF");
                }
                if (!context.EventType.Any())
                {
                    migrationBuilder.Sql("SET IDENTITY_INSERT dbo.EventType ON");
                    migrationBuilder.InsertData(table: "EventType",
                       columns: new[] { "Id", "Name", "IsActive", "CreatedDate", "ModifiedDate" },
                       values: new object[,] {
                { 1,"app started",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 2,"charge session started",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 3,"charge session ended",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 7,"charging session failed",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 8,"charging station connection failed",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 9,"charge station not returning the battery level",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 12,"video started",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 13,"video ended",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 14,"video failed",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },

                       });
                    migrationBuilder.Sql("SET IDENTITY_INSERT dbo.EventType OFF");
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
