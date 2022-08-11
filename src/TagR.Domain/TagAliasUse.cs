using Remora.Rest.Core;

namespace TagR.Domain;

public class TagAliasUse
{
    public int Id { get; set; }
    
    public Snowflake UserSnowflake { get; set; }
    
    public Snowflake ChannelSnowflake { get; set; }
    
    public DateTime DateTimeUtc { get; set; }

    public int TagAliasId { get; set; }
    
    public TagAlias TagAlias { get; set; }
}