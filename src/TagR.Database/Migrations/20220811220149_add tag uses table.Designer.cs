﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TagR.Database;

#nullable disable

namespace TagR.Database.Migrations
{
    [DbContext(typeof(TagRDbContext))]
    [Migration("20220811220149_add tag uses table")]
    partial class addtagusestable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TagR.Domain.AuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("action_type");

                    b.Property<long>("Actor")
                        .HasColumnType("bigint")
                        .HasColumnName("actor");

                    b.Property<string>("Details")
                        .HasColumnType("text")
                        .HasColumnName("details");

                    b.Property<int>("TagId")
                        .HasColumnType("integer")
                        .HasColumnName("tag_id");

                    b.Property<DateTime>("TimestampUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp_utc");

                    b.HasKey("Id")
                        .HasName("pk_tagr_auditlogs");

                    b.HasIndex("TagId")
                        .HasDatabaseName("ix_tagr_auditlogs_tag_id");

                    b.ToTable("tagr_auditlogs", (string)null);
                });

            modelBuilder.Entity("TagR.Domain.Moderation.BlockedUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BlockedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("blocked_at_utc");

                    b.Property<string>("Reason")
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.Property<long>("UserSnowflake")
                        .HasColumnType("bigint")
                        .HasColumnName("user_snowflake");

                    b.HasKey("Id")
                        .HasName("pk_tagr_blocked_users");

                    b.ToTable("tagr_blocked_users", (string)null);
                });

            modelBuilder.Entity("TagR.Domain.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at_utc");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean")
                        .HasColumnName("disabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("OwnerDiscordSnowflake")
                        .HasColumnType("bigint")
                        .HasColumnName("owner_discord_snowflake");

                    b.HasKey("Id")
                        .HasName("pk_tagr_tags");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_tagr_tags_name");

                    b.ToTable("tagr_tags", (string)null);
                });

            modelBuilder.Entity("TagR.Domain.TagAlias", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("ParentId")
                        .HasColumnType("integer")
                        .HasColumnName("parent_id");

                    b.Property<int>("Uses")
                        .HasColumnType("integer")
                        .HasColumnName("uses");

                    b.HasKey("Id")
                        .HasName("pk_tagr_aliases");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("ix_tagr_aliases_parent_id");

                    b.ToTable("tagr_aliases", (string)null);
                });

            modelBuilder.Entity("TagR.Domain.TagRevision", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hash");

                    b.Property<string>("ShortHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("short_hash");

                    b.Property<int>("TagId")
                        .HasColumnType("integer")
                        .HasColumnName("tag_id");

                    b.Property<DateTime>("TimestampUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp_utc");

                    b.HasKey("Id")
                        .HasName("pk_tagr_revisions");

                    b.HasIndex("TagId")
                        .HasDatabaseName("ix_tagr_revisions_tag_id");

                    b.ToTable("tagr_revisions", (string)null);
                });

            modelBuilder.Entity("TagR.Domain.TagUse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("ChannelSnowflake")
                        .HasColumnType("bigint")
                        .HasColumnName("channel_snowflake");

                    b.Property<DateTime>("DateTimeUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_time_utc");

                    b.Property<int>("TagId")
                        .HasColumnType("integer")
                        .HasColumnName("tag_id");

                    b.Property<long>("UserSnowflake")
                        .HasColumnType("bigint")
                        .HasColumnName("user_snowflake");

                    b.HasKey("Id")
                        .HasName("pk_tagr_uses");

                    b.HasIndex("TagId")
                        .HasDatabaseName("ix_tagr_uses_tag_id");

                    b.ToTable("tagr_uses", (string)null);
                });

            modelBuilder.Entity("TagR.Domain.AuditLog", b =>
                {
                    b.HasOne("TagR.Domain.Tag", "Tag")
                        .WithMany("AuditLogs")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_tagr_auditlogs_tags_tag_id");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("TagR.Domain.TagAlias", b =>
                {
                    b.HasOne("TagR.Domain.Tag", "Parent")
                        .WithMany("Aliases")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tagr_aliases_tags_parent_id");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("TagR.Domain.TagRevision", b =>
                {
                    b.HasOne("TagR.Domain.Tag", "Tag")
                        .WithMany("Revisions")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tagr_revisions_tagr_tags_tag_id");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("TagR.Domain.TagUse", b =>
                {
                    b.HasOne("TagR.Domain.Tag", "Tag")
                        .WithMany("Uses")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tagr_uses_tagr_tags_tag_id");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("TagR.Domain.Tag", b =>
                {
                    b.Navigation("Aliases");

                    b.Navigation("AuditLogs");

                    b.Navigation("Revisions");

                    b.Navigation("Uses");
                });
#pragma warning restore 612, 618
        }
    }
}