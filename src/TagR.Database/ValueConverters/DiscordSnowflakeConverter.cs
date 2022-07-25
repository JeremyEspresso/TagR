using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Remora.Rest.Core;

namespace TagR.Database.ValueConverters;

public class DiscordSnowflakeConverter : ValueConverter<Snowflake, long>
{
    private const ulong DiscordEpoch = 1420070400000;

    public DiscordSnowflakeConverter() : base
        (
            v => (long)v.Value,
            v => new Snowflake((ulong)v, DiscordEpoch)
        )
    {
    }
}
