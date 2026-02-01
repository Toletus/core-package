using Toletus.Pack.Core.Access.Logic.Enums;

namespace Toletus.Pack.Core.Access.Logic.Models;

/// <summary>
/// Input to the resolver.
/// This class should be easy to build from your Edge payload + configuration
/// + validation response.
/// </summary>
public sealed class AccessProcessingContext
{
    // -------------------------
    // Observed/known facts
    // -------------------------

    /// <summary>
    /// Direction detected for this event (Entry/Exit only).
    /// </summary>
    public required AccessLogicalDirectionEnum DetectedDirection { get; init; }

    /// <summary>
    /// The configured policy for that direction at the time of the event.
    /// </summary>
    public required DirectionPolicyEnum DirectionPolicy { get; init; }

    /// <summary>
    /// How access was initiated / identified (optional).
    /// </summary>
    public AccessMethodEnum? AccessMethod { get; init; }

    // -------------------------
    // Overrides / special signals
    // -------------------------

    /// <summary>
    /// True when emergency mode is active. Emergency overrides everything.
    /// </summary>
    public bool EmergencyActive { get; init; }

    // -------------------------
    // Identification (optional)
    // -------------------------

    /// <summary>
    /// Identified person (if known). Null when unknown.
    /// </summary>
    public Guid? PersonId { get; init; }

    /// <summary>
    /// Raw identifier (card number, QR payload, etc.) if you log it.
    /// </summary>
    public string? Identifier { get; init; }

    // -------------------------
    // Validation / decision
    // -------------------------

    /// <summary>
    /// Validation decision when policy is Controlled.
    /// Unknown means offline/no response/no decision.
    /// </summary>
    public ValidationDecisionEnum ValidationDecision { get; init; } = ValidationDecisionEnum.Unknown;

    /// <summary>
    /// If policy is Controlled and decision is Unknown (offline/no response),
    /// choose whether to allow passage as free policy.
    /// </summary>
    public bool OfflineFallbackAllowFree { get; init; } = false;
}