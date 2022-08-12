using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class removeblockeduserreason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reason",
                table: "tagr_blocked_users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "reason",
                table: "tagr_blocked_users",
                type: "text",
                nullable: true);
        }
    }
}
