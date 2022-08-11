using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagR.Domain;

namespace TagR.Database.EntityConfigurations;

public class TagAliasEntityConfiguration : IEntityTypeConfiguration<TagAlias>
{
    public void Configure(EntityTypeBuilder<TagAlias> builder)
    {
        builder.ToTable("tagr_aliases");

        builder.HasMany(x => x.Uses)
            .WithOne(x => x.TagAlias)
            .HasForeignKey(x => x.TagAliasId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}