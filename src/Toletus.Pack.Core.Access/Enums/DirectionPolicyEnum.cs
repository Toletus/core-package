namespace Toletus.Pack.Core.Access.Enums;

/// <summary>
/// Policy for a *specific direction* (Entry or Exit) defined in device config.
/// This enum originally comes from configuration, but we also use it as an input
/// to understand what was expected behavior when the passage happened.
/// </summary>
public enum DirectionPolicyEnum
{
    /// <summary>Direction is blocked (no passage allowed).</summary>
    Blocked,

    /// <summary>Direction is free (passage allowed without validation).</summary>
    Free,

    /// <summary>Direction is controlled (requires validation decision).</summary>
    Controlled,
}