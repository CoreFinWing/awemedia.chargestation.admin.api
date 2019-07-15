using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedStatusForPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Notification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notification");
        }
    }
}
