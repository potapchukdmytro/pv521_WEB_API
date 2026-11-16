using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PV521_BooksShop.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261116130000_UniqueRoleName")]
    public class UniqueRoleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE UNIQUE INDEX "IX_Roles_Name_CaseInsensitive"
                ON "Roles" (lower("Name"));
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """DROP INDEX "IX_Roles_Name_CaseInsensitive";""");
        }
    }
}
