using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TagR.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tagr_tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    owner_discord_snowflake = table.Column<long>(type: "bigint", nullable: false),
                    uses = table.Column<int>(type: "integer", nullable: false),
                    disabled = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tagr_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tagr_auditlogs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    action_type = table.Column<string>(type: "text", nullable: false),
                    actor = table.Column<long>(type: "bigint", nullable: false),
                    details = table.Column<string>(type: "text", nullable: false),
                    timestamp_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tagr_auditlogs", x => x.id);
                    table.ForeignKey(
                        name: "fk_tagr_auditlogs_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tagr_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tagr_auditlogs_tag_id",
                table: "tagr_auditlogs",
                column: "tag_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tagr_auditlogs");

            migrationBuilder.DropTable(
                name: "tagr_tags");
        }
    }
}
