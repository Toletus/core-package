namespace Toletus.Pack.Core.Access.Enums;

/// <summary>
/// Describes the technical credential used for *automatic identification*.
///
/// This enum is ONLY applicable when:
///   AccessTriggerKind == AutomaticCredential
///
/// IMPORTANT:
/// - This enum must NEVER be used for manual releases.
/// - It represents the technical identification mechanism,
///   not a human decision or target.
/// </summary>
public enum AccessCredentialMethod
{
    /// <summary>
    /// Proximity or smart card credential.
    /// </summary>
    Card,

    /// <summary>
    /// Facial recognition.
    /// </summary>
    Face,

    /// <summary>
    /// Fingerprint biometric identification.
    /// </summary>
    Fingerprint,

    /// <summary>
    /// QR Code presented by the person.
    /// </summary>
    QRCode,

    /// <summary>
    /// Bluetooth-based identification (mobile device).
    /// </summary>
    Bluetooth,

    /// <summary>
    /// Keyboard or PIN-based identification.
    /// </summary>
    Keyboard
}