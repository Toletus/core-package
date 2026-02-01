using Toletus.Pack.Core.Access.Logic.Enums;

namespace Toletus.Pack.Core.Access.Logic.Models;

/// <summary>
/// Output of the resolver (what you persist/log).
/// This is normalized and consistent.
/// </summary>
public sealed class AccessResolvedRecord
{
    public required AccessLogicalDirectionEnum Direction { get; init; }
    public required AccessControlOutcomeEnum Outcome { get; init; }
    public required PassageReleaseKindEnum ReleaseKind { get; init; }

    /// <summary>
    /// Convenience boolean for BI / filtering.
    /// Derived from Outcome: allowed when Outcome != Blocked.
    /// </summary>
    public required bool IsAllowed { get; init; }

    /// <summary>
    /// Stable key for later localization. Persist this, not the final text.
    /// </summary>
    public required AccessSummaryKey SummaryKey { get; init; }

    /// <summary>
    /// Optional context for later display/analysis.
    /// </summary>
    public AccessMethodEnum? AccessMethod { get; init; }

    public Guid? PersonId { get; init; }
    public string? Identifier { get; init; }
}