namespace Toletus.Pack.Core.Access.Enums;

/// <summary>
/// Logical direction observed for this event.
/// NOTE: For an event log, it must be Entry or Exit (Both is only for config).
/// </summary>
public enum AccessLogicalDirectionEnum
{
    Entry = 1,
    Exit  = 2,

    /// <summary>
    /// Config helper only (bitwise). MUST NOT appear as an event direction.
    /// </summary>
    Both  = Entry | Exit
}