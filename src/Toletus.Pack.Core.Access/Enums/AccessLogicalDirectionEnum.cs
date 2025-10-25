namespace Toletus.Pack.Core.Access.Enums;

[Flags]
public enum AccessLogicalDirectionEnum
{
    Entry = 1,
    Exit  = 2,
    Both  = Entry | Exit  // 3
}