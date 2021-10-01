using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedCountryIdColumnInChargeOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "ChargeOptions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeOptions_CountryId",
                table: "ChargeOptions",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeOptions_Country",
                table: "ChargeOptions",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeOptions_Country",
                table: "ChargeOptions");

            migrationBuilder.DropIndex(
                name: "IX_ChargeOptions_CountryId",
                table: "ChargeOptions");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "ChargeOptions");
        }
    }
}
