using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagR.Domain;

namespace TagR.Database.EntityConfigurations;

public class TagRevisionEntityConfiguration : IEntityTypeConfiguration<TagRevision>
{
    public void Configure(EntityTypeBuilder<TagRevision> builder)
    {
        builder.ToTable("tagr_revisions");
    }
}