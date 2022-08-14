using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class adduniqueconstraintaliasname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_tagr_aliases_name",
                table: "tagr_aliases",
                column: "name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tagr_aliases_name",
                table: "tagr_aliases");
        }
    }
}
