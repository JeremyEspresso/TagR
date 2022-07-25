using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TagR.Domain;

namespace TagR.Database.ValueConverters;

public class TagAuditLogActionConverter : ValueConverter<TagAuditLogAction, string>
{
    public TagAuditLogActionConverter() : base
        (
            v => v.ToString(),
            v => (TagAuditLogAction)Enum.Parse(typeof(TagAuditLogAction), v) // TODO: Implement EnumGen
        )
    {
    }
}

