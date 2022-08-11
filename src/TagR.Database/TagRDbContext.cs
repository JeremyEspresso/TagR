using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using TagR.Database.ValueConverters;
using TagR.Domain;
using TagR.Domain.Moderation;

namespace TagR.Database;

public class TagRDbContext : DbContext
{

    public TagRDbContext(DbContextOptions<TagRDbContext> options) : base(options) 
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder
        .UseNpgsql()
        .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TagRDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Snowflake>()
            .HaveConversion<DiscordSnowflakeConverter>();

        configurationBuilder
            .Properties<TagAuditLogAction>()
            .HaveConversion<TagAuditLogActionConverter>();
    }

    public DbSet<Tag> Tags { get; set; } = default!;

    public DbSet<AuditLog> AuditLogs { get; set; } = default!;

    public DbSet<BlockedUser> BlockedUsers { get; set; } = default!;

    public DbSet<TagRevision> Revisions { get; set; } = default!;

    public DbSet<TagAlias> Aliases { get; set; } = default!;

    public DbSet<TagUse> Uses { get; set; } = default!;
    
    public DbSet<TagAliasUse> AliasUses { get; set; } = default!;
}
