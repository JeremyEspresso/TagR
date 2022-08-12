using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagR.Domain;

namespace TagR.Database.EntityConfigurations;

public class TagUseEntityConfiguration : IEntityTypeConfiguration<TagUse>
{
    public void Configure(EntityTypeBuilder<TagUse> builder)
    {
        builder.ToTable("tagr_uses");
    }
}