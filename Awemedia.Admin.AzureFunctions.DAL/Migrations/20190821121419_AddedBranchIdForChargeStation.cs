using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedBranchIdForChargeStation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeStation_Merchant",
                table: "ChargeStation");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ChargeStation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStation_BranchId",
                table: "ChargeStation",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeStation_Branch",
                table: "ChargeStation",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeStation_Merchant_MerchantId",
                table: "ChargeStation",
                column: "MerchantId",
                principalTable: "Merchant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeStation_Branch",
                table: "ChargeStation");

            migrationBuilder.DropForeignKey(
                name: "FK_ChargeStation_Merchant_MerchantId",
                table: "ChargeStation");

            migrationBuilder.DropIndex(
                name: "IX_ChargeStation_BranchId",
                table: "ChargeStation");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ChargeStation");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeStation_Merchant",
                table: "ChargeStation",
                column: "MerchantId",
                principalTable: "Merchant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
