namespace Toletus.Pack.Core.Access.Logic.Enums;

/// <summary>
/// Validation decision (from cloud or local rules).
/// This is the decision relevant when policy is Controlled.
/// </summary>
public enum ValidationDecisionEnum
{
    Unknown = 0, // offline/no response/no decision
    Granted,
    Denied
}