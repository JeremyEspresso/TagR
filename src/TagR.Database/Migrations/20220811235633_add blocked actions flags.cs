using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class addblockedactionsflags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "blocked_actions",
                table: "tagr_blocked_users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "blocked_actions",
                table: "tagr_blocked_users");
        }
    }
}
