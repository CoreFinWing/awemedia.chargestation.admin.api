using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class RemovedPromotionTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotion_PromotionType",
                table: "Promotion");

            migrationBuilder.DropTable(
                name: "PromotionType");

            migrationBuilder.DropIndex(
                name: "IX_Promotion_PromotionTypeId",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "PromotionTypeId",
                table: "Promotion");

            migrationBuilder.AddColumn<string>(
                name: "PromotionType",
                table: "Promotion",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionType",
                table: "Promotion");

            migrationBuilder.AddColumn<int>(
                name: "PromotionTypeId",
                table: "Promotion",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PromotionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PromotionType = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_PromotionTypeId",
                table: "Promotion",
                column: "PromotionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotion_PromotionType",
                table: "Promotion",
                column: "PromotionTypeId",
                principalTable: "PromotionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
