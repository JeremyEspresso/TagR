using Remora.Rest.Core;

namespace TagR.Domain;

public class TagUse
{
    public int Id { get; set; }
    
    public Snowflake UserSnowflake { get; set; }
    
    public Snowflake ChannelSnowflake { get; set; }
    
    public DateTime DateTimeUtc { get; set; }
    
    public int TagId { get; set; }

    public Tag Tag { get; set; }
}