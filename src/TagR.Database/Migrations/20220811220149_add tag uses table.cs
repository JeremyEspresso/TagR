using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class addtagusestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uses",
                table: "tagr_tags");

            migrationBuilder.CreateTable(
                name: "tagr_uses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_snowflake = table.Column<long>(type: "bigint", nullable: false),
                    channel_snowflake = table.Column<long>(type: "bigint", nullable: false),
                    date_time_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tagr_uses", x => x.id);
                    table.ForeignKey(
                        name: "fk_tagr_uses_tagr_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tagr_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tagr_uses_tag_id",
                table: "tagr_uses",
                column: "tag_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tagr_uses");

            migrationBuilder.AddColumn<int>(
                name: "uses",
                table: "tagr_tags",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
