using System;
using Toletus.Pack.Core.Access.Enums;
using Toletus.Pack.Core.Access.Logic.Enums;
using Toletus.Pack.Core.Access.Logic.Localization;
using Toletus.Pack.Core.Access.Logic.Models;
using Toletus.Pack.Core.Access.Logic.Resolvers;

namespace Toletus.Pack.Core.Access.Logic;

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