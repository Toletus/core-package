namespace Toletus.Pack.Core.Access.Logic.Enums;

/// <summary>
/// Why/how the passage was released (mechanism/cause).
/// This is NOT "who" or "what credential"; it describes the release mechanism.
/// </summary>
public enum PassageReleaseKindEnum
{
    /// <summary>
    /// System-controlled decision (rules/validation). Can grant or deny.
    /// </summary>
    AutomaticControl,

    /// <summary>
    /// Passage happened because the configured policy allowed "Free" mode
    /// (or because we used offline fallback to allow free).
    /// </summary>
    PolicyFree,

    /// <summary>
    /// Explicit operator command in the software (e.g., F2/F3) released the turnstile.
    /// IMPORTANT: manual release only exists when the operator commanded it.
    /// </summary>
    ManualRelease,

    /// <summary>
    /// Emergency mode (evacuation / fire alarm) released passage.
    /// </summary>
    Emergency
}