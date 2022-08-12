using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagR.Domain;

namespace TagR.Database.EntityConfigurations;

public class TagAliasUseEntityConfiguration : IEntityTypeConfiguration<TagAliasUse>
{
    public void Configure(EntityTypeBuilder<TagAliasUse> builder)
    {
        builder.ToTable("tagr_alias_uses");
    }
}