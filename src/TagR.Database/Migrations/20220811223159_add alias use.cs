using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class addaliasuse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uses",
                table: "tagr_aliases");

            migrationBuilder.CreateTable(
                name: "tagr_alias_uses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_snowflake = table.Column<long>(type: "bigint", nullable: false),
                    channel_snowflake = table.Column<long>(type: "bigint", nullable: false),
                    date_time_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tag_alias_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tagr_alias_uses", x => x.id);
                    table.ForeignKey(
                        name: "fk_tagr_alias_uses_tagr_aliases_tag_alias_id",
                        column: x => x.tag_alias_id,
                        principalTable: "tagr_aliases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tagr_alias_uses_tag_alias_id",
                table: "tagr_alias_uses",
                column: "tag_alias_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tagr_alias_uses");

            migrationBuilder.AddColumn<int>(
                name: "uses",
                table: "tagr_aliases",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
