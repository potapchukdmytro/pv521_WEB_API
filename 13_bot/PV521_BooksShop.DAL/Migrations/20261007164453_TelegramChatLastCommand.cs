using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PV521_BooksShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TelegramChatLastCommand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastCommand",
                table: "TelegramChats",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCommand",
                table: "TelegramChats");
        }
    }
}
