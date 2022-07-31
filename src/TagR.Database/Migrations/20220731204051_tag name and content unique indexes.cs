using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class tagnameandcontentuniqueindexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_tagr_tags_content",
                table: "tagr_tags",
                column: "content",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tagr_tags_name",
                table: "tagr_tags",
                column: "name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tagr_tags_content",
                table: "tagr_tags");

            migrationBuilder.DropIndex(
                name: "ix_tagr_tags_name",
                table: "tagr_tags");
        }
    }
}
