namespace TagR.Domain;

/// <summary>
/// A action indicating an auditlog action type.
/// </summary>
public enum TagAuditLogAction
{
    /// <summary>
    /// New tag was created
    /// </summary>
    Create,

    /// <summary>
    /// Existing tag was updated (edited)
    /// </summary>
    Update,

    /// <summary>
    /// Existing tag was deleted
    /// </summary>
    Delete,

    /// <summary>
    /// Existing tag was enabled
    /// </summary>
    Enable,

    /// <summary>
    /// Existing tag was disabled
    /// </summary>
    Disable,

    /// <summary>
    /// Alias was created for an existing tag
    /// </summary>
    Alias
}
