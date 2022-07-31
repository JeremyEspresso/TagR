using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagR.Domain.Moderation;

namespace TagR.Database.EntityConfigurations;

public class BlockedUserEntityConfiguration : IEntityTypeConfiguration<BlockedUser>
{
    public void Configure(EntityTypeBuilder<BlockedUser> builder)
    {
        builder.ToTable("tagr_blocked_users");
    }
}
