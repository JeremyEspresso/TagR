using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class addlockedstatetotagmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "locked",
                table: "tagr_tags",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "locked",
                table: "tagr_tags");
        }
    }
}
