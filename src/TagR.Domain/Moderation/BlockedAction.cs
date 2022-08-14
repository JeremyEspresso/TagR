namespace TagR.Domain.Moderation;

[Flags]
public enum BlockedAction
{
  None = 0,
  
  TagCreate = 1,
  TagEdit = 2,
  TagDelete = 4,
  TagModify = TagEdit | TagDelete,
  TagInvoke = 8,
  
  AliasCreate = 10,
  AliasEdit = 12, 
  AliasDelete = 14,
  AliasModify = AliasEdit | AliasDelete,
  AliasInvoke = TagInvoke
}
