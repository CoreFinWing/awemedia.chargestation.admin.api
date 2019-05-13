using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedColumnUIDAndDeviceToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "ChargeStation",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceToken",
                table: "ChargeStation",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UID",
                table: "ChargeStation",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStations_DeviceId",
                table: "ChargeStation",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStations_UID",
                table: "ChargeStation",
                column: "UID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChargeStations_DeviceId",
                table: "ChargeStation");

            migrationBuilder.DropIndex(
                name: "IX_ChargeStations_UID",
                table: "ChargeStation");

            migrationBuilder.DropColumn(
                name: "DeviceToken",
                table: "ChargeStation");

            migrationBuilder.DropColumn(
                name: "UID",
                table: "ChargeStation");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "ChargeStation",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);
        }
    }
}
