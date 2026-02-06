namespace Toletus.Pack.Core.Access.Enums;

/// <summary>
/// Describes how the access was triggered.
/// This enum represents the *decision path*, not the technical means.
///
/// IMPORTANT:
/// - This enum does NOT represent a credential or identification method.
/// - It describes whether the access was granted automatically by the system
///   or manually by a human operator.
///
/// This is a core audit dimension and must remain stable over time.
/// </summary>
public enum AccessTriggerKind
{
    /// <summary>
    /// Access was granted automatically after successful credential validation
    /// (e.g. card, face, fingerprint, QR code).
    ///
    /// No human decision at the moment of release.
    /// </summary>
    AutomaticCredential,

    /// <summary>
    /// Access was granted manually by an operator without selecting
    /// a specific person (anonymous manual release).
    ///
    /// Examples:
    /// - Physical shortcut (F2/F3)
    /// - Manual override without person context
    /// </summary>
    ManualAnonymous,

    /// <summary>
    /// Access was granted manually by an operator targeting
    /// a specific known person.
    ///
    /// IMPORTANT:
    /// - The person is the *target* of the release, NOT the operator.
    /// - No automatic credential validation occurs in this mode.
    /// - Typically initiated from the person's profile in the SaaS.
    /// </summary>
    ManualTargeted,
    
    /// <summary>
    /// Access was originally denied by the system and later
    /// explicitly overridden by a human operator.
    ///
    /// This represents a conscious bypass of an automatic decision.
    ///
    /// IMPORTANT:
    /// - A prior automatic evaluation exists.
    /// - The original denial reason must be preserved in audit logs.
    /// - This is NOT a simple manual release.
    /// </summary>
    ManualOverrideDenied
}