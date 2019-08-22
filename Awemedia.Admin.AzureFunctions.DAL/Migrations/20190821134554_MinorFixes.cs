using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class MinorFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeStation_Merchant_MerchantId",
                table: "ChargeStation");

            migrationBuilder.DropIndex(
                name: "IX_ChargeStation_MerchantId",
                table: "ChargeStation");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "ChargeStation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MerchantId",
                table: "ChargeStation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStation_MerchantId",
                table: "ChargeStation",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeStation_Merchant_MerchantId",
                table: "ChargeStation",
                column: "MerchantId",
                principalTable: "Merchant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
