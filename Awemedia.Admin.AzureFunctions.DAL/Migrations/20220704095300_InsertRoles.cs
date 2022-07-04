using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class InsertRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('user', 'User', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('sales', 'Sales', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('salessupport', 'Sales Support', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('techsupport', 'Tech Support', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('operationsupport', 'Operation Support', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('financesupport', 'Finance Support', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('admin', 'Admin', 1)");
            migrationBuilder.Sql("INSERT [dbo].[Role] ([Name], [DisplayName], [IsActive]) VALUES ('superadmin', 'Super Admin', 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
