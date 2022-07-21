using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class UpdateRoleNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [User]");
            migrationBuilder.Sql("Delete from [Role]");

            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('user', 'User', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('sales', 'Sales', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('support', 'Support', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('owner', 'Owner', 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
