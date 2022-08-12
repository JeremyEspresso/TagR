namespace TagR.Domain.Moderation;

[Flags]
public enum BlockedAction
{
  None = 0,
  TagModify = 1,
  TagInvoke = 2,
}
