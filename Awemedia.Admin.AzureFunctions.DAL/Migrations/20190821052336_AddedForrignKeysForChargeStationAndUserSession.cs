using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedForrignKeysForChargeStationAndUserSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "fkIdx_SessionType",
                table: "UserSession",
                newName: "IX_UserSession_SessionType");

            migrationBuilder.RenameIndex(
                name: "fkIdx_SessionStatus",
                table: "UserSession",
                newName: "IX_UserSession_SessionStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "ChargeStationId",
                table: "UserSession",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "UserSession",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MerchantId",
                table: "ChargeStation",
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_ChargeStationId",
                table: "UserSession",
                column: "ChargeStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStation_MerchantId",
                table: "ChargeStation",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeStation_Merchant",
                table: "ChargeStation",
                column: "MerchantId",
                principalTable: "Merchant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSession_ChargeStation",
                table: "UserSession",
                column: "ChargeStationId",
                principalTable: "ChargeStation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeStation_Merchant",
                table: "ChargeStation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSession_ChargeStation",
                table: "UserSession");

            migrationBuilder.DropIndex(
                name: "IX_UserSession_ChargeStationId",
                table: "UserSession");

            migrationBuilder.DropIndex(
                name: "IX_ChargeStation_MerchantId",
                table: "ChargeStation");

            migrationBuilder.DropColumn(
                name: "ChargeStationId",
                table: "UserSession");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "UserSession");

            migrationBuilder.RenameIndex(
                name: "IX_UserSession_SessionType",
                table: "UserSession",
                newName: "fkIdx_SessionType");

            migrationBuilder.RenameIndex(
                name: "IX_UserSession_SessionStatus",
                table: "UserSession",
                newName: "fkIdx_SessionStatus");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantId",
                table: "ChargeStation",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
