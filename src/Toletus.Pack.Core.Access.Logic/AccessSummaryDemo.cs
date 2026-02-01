using System;
using Toletus.Pack.Core.Access.Logic.Enums;
using Toletus.Pack.Core.Access.Logic.Localization;
using Toletus.Pack.Core.Access.Logic.Models;
using Toletus.Pack.Core.Access.Logic.Resolvers;

namespace Toletus.Pack.Core.Access.Logic
{
    // =========================================================================
    // ✅ PURPOSE OF THIS FILE
    // =========================================================================
    // This module centralizes the *domain logic* for turnstile passages:
    // - It receives configuration (DirectionPolicy) + signals (Emergency, Manual cmd)
    //   + identification/validation info (PersonId/Identifier, ValidationDecision)
    // - It outputs a normalized result (Outcome, ReleaseKind, IsAllowed) and a
    //   stable SummaryKey that can be persisted (no localized text in the DB).
    //
    // Localized text (pt-BR / es / en) is produced later by AccessSummaryLocalizer,
    // using SummaryKey + direction + method + isIdentified.
    //
    // IMPORTANT:
    // - The resolver is the "source of truth" for decisions.
    // - The localizer is ONLY for presentation (UX, logs, UI), never for decisions.
    // =========================================================================


    // =========================================================================
    // 1) CONFIGURATION ENUMS (how the turnstile is configured to behave)
    // =========================================================================


    // =========================================================================
    // 2) EVENT / LOG ENUMS (what actually happened)
    // =========================================================================


    // =========================================================================
    // 3) INPUT (context) & OUTPUT (resolved record)
    // =========================================================================


    // =========================================================================
    // 4) RESOLVER (domain decision engine)
    // =========================================================================


    // =========================================================================
    // 5) LOCALIZER (presentation-only, multilingual summaries)
    // =========================================================================

    // =========================================================================
    // 6) Demo usage (optional)
    // =========================================================================

    public static class AccessSummaryDemo
    {
        public static void Run()
        {
            // Example: build context (comes from device + config + validation)
            var ctx = new AccessProcessingContext
            {
                DetectedDirection = AccessLogicalDirectionEnum.Entry,
                DirectionPolicy = DirectionPolicyEnum.Controlled,
                AccessMethod = AccessMethodEnum.Face,
                PersonId = Guid.Parse("8b6f7f6a-8c5a-4e2a-9e31-8c8a1e6a2d01"),
                Identifier = "FACE:match-9981",
                ValidationDecision = ValidationDecisionEnum.Granted
            };

            // Resolve domain decision (this is what you persist)
            var resolved = AccessOutcomeResolver.Resolve(ctx);

            // Later: render localized text in UI/logs based on tenant/user language
            var pt = AccessSummaryLocalizer.Format(
                lang: "pt-BR",
                key: resolved.SummaryKey,
                direction: resolved.Direction,
                method: resolved.AccessMethod,
                isIdentified: resolved.PersonId.HasValue || !string.IsNullOrWhiteSpace(resolved.Identifier));

            Console.WriteLine($"SummaryKey persisted: {resolved.SummaryKey}");
            Console.WriteLine($"PT-BR: {pt}");
        }
    }
}
