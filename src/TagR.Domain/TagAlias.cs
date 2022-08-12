namespace TagR.Domain;

public class TagAlias
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public int ParentId { get; set; }

    public Tag Parent { get; set; }

    public ICollection<TagAliasUse> Uses { get; set; } = new List<TagAliasUse>();
}