namespace Toletus.Pack.Core.Access.Logic.Enums;

/// <summary>
/// Stable, persisted key used to render localized summary later.
/// Store this in DB instead of storing localized text.
/// </summary>
public enum AccessSummaryKey
{
    /// <summary>Authorized by validation (controlled passage).</summary>
    Controlled,

    /// <summary>Attempt blocked by access control.</summary>
    Blocked,

    /// <summary>Free by policy/configuration (or offline fallback free).</summary>
    FreePolicy,

    /// <summary>Free because operator issued a manual release command (F2/F3).</summary>
    FreeManual,

    /// <summary>Free due to emergency.</summary>
    FreeEmergency
}