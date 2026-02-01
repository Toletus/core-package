namespace Toletus.Pack.Core.Access.Logic.Enums;

/// <summary>
/// Outcome of the passage attempt.
/// - Controlled: passage allowed after validation
/// - Free: passage allowed without validation (policy free / manual / emergency)
/// - Blocked: attempt blocked (no passage)
/// </summary>
public enum AccessControlOutcomeEnum
{
    Blocked,
    Free,
    Controlled,
}