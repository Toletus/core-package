using Toletus.Pack.Core.Access.Enums;
using Toletus.Pack.Core.Access.Logic.Enums;
using Toletus.Pack.Core.Access.Logic.Models;

namespace Toletus.Pack.Core.Access.Logic.Resolvers;

/// <summary>
/// Core resolver: transforms context into a consistent access record.
/// It DOES NOT generate localized text; it generates SummaryKey.
/// </summary>
public static class AccessOutcomeResolver
{
    public static AccessResolvedRecord Resolve(AccessProcessingContext ctx)
    {
        // Ensure event direction is valid (Entry/Exit only).
        var direction = NormalizeEventDirection(ctx.DetectedDirection);

        // Manual release is ONLY when AccessMethod is an explicit operator command (F2/F3).
        bool isManualReleaseCommand = IsManualReleaseCommand(ctx.AccessMethod);

        // "Identified" means we have PersonId or some identifier string.
        bool isIdentified = ctx.PersonId.HasValue || !string.IsNullOrWhiteSpace(ctx.Identifier);

        // -----------------------------------------------------------------
        // 1) Compute Outcome + ReleaseKind with precedence
        // -----------------------------------------------------------------
        // Precedence rules:
        // 1) Emergency overrides everything
        // 2) Manual operator command (F2/F3) overrides policy
        // 3) Otherwise use policy + validation decision (if Controlled)
        // -----------------------------------------------------------------

        AccessControlOutcomeEnum outcome;
        PassageReleaseKindEnum releaseKind;

        if (ctx.EmergencyActive)
        {
            outcome = AccessControlOutcomeEnum.Free;
            releaseKind = PassageReleaseKindEnum.Emergency;
        }
        else if (isManualReleaseCommand)
        {
            outcome = AccessControlOutcomeEnum.Free;
            releaseKind = PassageReleaseKindEnum.ManualRelease;
        }
        else
        {
            switch (ctx.DirectionPolicy)
            {
                case DirectionPolicyEnum.Free:
                    outcome = AccessControlOutcomeEnum.Free;
                    releaseKind = PassageReleaseKindEnum.PolicyFree;
                    break;

                case DirectionPolicyEnum.Blocked:
                    outcome = AccessControlOutcomeEnum.Blocked;
                    releaseKind = PassageReleaseKindEnum.AutomaticControl;
                    break;

                case DirectionPolicyEnum.Controlled:
                default:
                    // In controlled mode we depend on ValidationDecision.
                    if (ctx.ValidationDecision == ValidationDecisionEnum.Granted)
                    {
                        outcome = AccessControlOutcomeEnum.Controlled;
                        releaseKind = PassageReleaseKindEnum.AutomaticControl;
                    }
                    else if (ctx.ValidationDecision == ValidationDecisionEnum.Denied)
                    {
                        outcome = AccessControlOutcomeEnum.Blocked;
                        releaseKind = PassageReleaseKindEnum.AutomaticControl;
                    }
                    else
                    {
                        // Unknown decision (offline/no response). Apply fallback behavior.
                        if (ctx.OfflineFallbackAllowFree)
                        {
                            outcome = AccessControlOutcomeEnum.Free;
                            releaseKind = PassageReleaseKindEnum.PolicyFree;
                        }
                        else
                        {
                            outcome = AccessControlOutcomeEnum.Blocked;
                            releaseKind = PassageReleaseKindEnum.AutomaticControl;
                        }
                    }
                    break;
            }
        }

        // -----------------------------------------------------------------
        // 2) Enforce invariants (keep logs consistent / prevent impossible states)
        // -----------------------------------------------------------------

        // Invariant A:
        // Controlled implies identification.
        // If we somehow got Controlled without identity, degrade to FreePolicy.
        if (outcome == AccessControlOutcomeEnum.Controlled && !isIdentified)
        {
            outcome = AccessControlOutcomeEnum.Free;
            releaseKind = PassageReleaseKindEnum.PolicyFree;
        }

        // Invariant B:
        // If outcome is Controlled, releaseKind must be AutomaticControl.
        if (outcome == AccessControlOutcomeEnum.Controlled)
            releaseKind = PassageReleaseKindEnum.AutomaticControl;

        // Invariant C:
        // If outcome is Free, releaseKind must NOT be AutomaticControl.
        if (outcome == AccessControlOutcomeEnum.Free && releaseKind == PassageReleaseKindEnum.AutomaticControl)
            releaseKind = PassageReleaseKindEnum.PolicyFree;

        // Invariant D (strict):
        // If method indicates manual release, force Free+ManualRelease.
        if (isManualReleaseCommand)
        {
            outcome = AccessControlOutcomeEnum.Free;
            releaseKind = PassageReleaseKindEnum.ManualRelease;
        }

        // -----------------------------------------------------------------
        // 3) Derive IsAllowed
        // -----------------------------------------------------------------
        bool isAllowed = outcome != AccessControlOutcomeEnum.Blocked;

        // -----------------------------------------------------------------
        // 4) Derive SummaryKey (persisted, stable)
        // -----------------------------------------------------------------
        var summaryKey = ResolveSummaryKey(outcome, releaseKind);

        // -----------------------------------------------------------------
        // 5) Return normalized record
        // -----------------------------------------------------------------
        return new AccessResolvedRecord
        {
            Direction = direction,
            Outcome = outcome,
            ReleaseKind = releaseKind,
            IsAllowed = isAllowed,
            SummaryKey = summaryKey,
            AccessMethod = ctx.AccessMethod,
            PersonId = ctx.PersonId,
            Identifier = ctx.Identifier
        };
    }

    // -------------------------
    // SummaryKey mapping
    // -------------------------

    private static AccessSummaryKey ResolveSummaryKey(
        AccessControlOutcomeEnum outcome,
        PassageReleaseKindEnum releaseKind)
    {
        // SummaryKey is intentionally coarse; method/direction are parameters for localization.
        return outcome switch
        {
            AccessControlOutcomeEnum.Controlled => AccessSummaryKey.Controlled,
            AccessControlOutcomeEnum.Blocked    => AccessSummaryKey.Blocked,

            AccessControlOutcomeEnum.Free when releaseKind == PassageReleaseKindEnum.Emergency
                => AccessSummaryKey.FreeEmergency,

            AccessControlOutcomeEnum.Free when releaseKind == PassageReleaseKindEnum.ManualRelease
                => AccessSummaryKey.FreeManual,

            AccessControlOutcomeEnum.Free
                => AccessSummaryKey.FreePolicy,

            _ => AccessSummaryKey.Blocked
        };
    }

    // -------------------------
    // Helpers
    // -------------------------

    private static bool IsManualReleaseCommand(AccessMethodEnum? method)
        => method == AccessMethodEnum.F2ManualEntryRelease
           || method == AccessMethodEnum.F3ManualExitRelease;

    private static AccessLogicalDirectionEnum NormalizeEventDirection(AccessLogicalDirectionEnum detected)
    {
        if (detected == AccessLogicalDirectionEnum.Both)
            throw new ArgumentException("DetectedDirection cannot be Both for an event log. Use Entry or Exit.");

        return detected;
    }
}