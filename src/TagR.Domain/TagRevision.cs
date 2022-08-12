namespace TagR.Domain;

public class TagRevision
{
    public int Id { get; set; }
    
    public string Hash { get; set; }
    
    public string ShortHash { get; set; }
    
    public DateTime TimestampUtc { get; set; }
    
    public string Content { get; set; }
    
    public int TagId { get; set; }
    
    public Tag Tag { get; set; }
}