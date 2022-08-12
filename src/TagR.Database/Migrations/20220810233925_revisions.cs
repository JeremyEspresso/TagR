using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class revisions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tagr_tags_content",
                table: "tagr_tags");

            migrationBuilder.DropColumn(
                name: "content",
                table: "tagr_tags");

            migrationBuilder.CreateTable(
                name: "tagr_revisions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hash = table.Column<string>(type: "text", nullable: false),
                    short_hash = table.Column<string>(type: "text", nullable: false),
                    timestamp_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tagr_revisions", x => x.id);
                    table.ForeignKey(
                        name: "fk_tagr_revisions_tagr_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tagr_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tagr_revisions_tag_id",
                table: "tagr_revisions",
                column: "tag_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tagr_revisions");

            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "tagr_tags",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_tagr_tags_content",
                table: "tagr_tags",
                column: "content",
                unique: true);
        }
    }
}
