using Toletus.Pack.Core.Access.Logic.Enums;

namespace Toletus.Pack.Core.Access.Logic.Localization;

/// <summary>
/// Presentation-only localizer:
/// - input: SummaryKey + direction + access method + identification flag
/// - output: localized summary string (pt-BR, es, en)
///
/// DO NOT use this string to drive decisions.
/// Always persist SummaryKey instead of persisting this text.
/// </summary>
public static class AccessSummaryLocalizer
{
    public static string Format(
        string lang,                     // "pt-BR", "es", "en" (fallback: "en")
        AccessSummaryKey key,            // persisted key
        AccessLogicalDirectionEnum direction,
        AccessMethodEnum? method,
        bool isIdentified)
    {
        var locale = NormalizeLang(lang);

        // Direction labels (localized)
        string dirTitle = GetDirTitle(locale, direction); // "Entrada"/"Saída" | "Entrada"/"Salida" | "Entry"/"Exit"
        string dirLower = GetDirLower(locale, direction); // lowercase for "tentativa de ..."

        // Method label (localized)
        string? methodLabel = GetMethodLabel(locale, method);

        // Method classification
        bool isIdMethod   = IsIdentificationMethod(method);
        bool isManualCmd  = IsManualCommandMethod(method);

        // Nuance rules:
        // - Controlled => "validated via X" (when method is an identification method)
        // - FreePolicy + identified + id-method => "identified via X" (NOT validated)
        // - FreeManual => mention F2/F3 when present
        return key switch
        {
            AccessSummaryKey.Controlled    => BuildControlled(locale, dirTitle, methodLabel, isIdMethod),
            AccessSummaryKey.Blocked       => BuildBlocked(locale, dirLower, methodLabel, isIdMethod),
            AccessSummaryKey.FreeEmergency => BuildEmergency(locale, dirTitle),
            AccessSummaryKey.FreeManual    => BuildFreeManual(locale, dirTitle, methodLabel, isManualCmd, isIdentified),
            AccessSummaryKey.FreePolicy    => BuildFreePolicy(locale, dirTitle, methodLabel, isIdMethod, isIdentified),
            _                              => BuildFallback(locale, dirTitle)
        };
    }

    // -------------------------
    // Templates by key
    // -------------------------

    private static string BuildControlled(string locale, string dirTitle, string? methodLabel, bool isIdMethod)
    {
        // Controlled => authorized by validation.
        // If we have an identification method label, it's safe to say "validated via ...".
        if (!string.IsNullOrWhiteSpace(methodLabel) && isIdMethod)
        {
            return locale switch
            {
                "pt-BR" => $"{dirTitle} autorizada após validação por {methodLabel}.",
                "es"    => $"{dirTitle} autorizada tras la validación por {methodLabel}.",
                _       => $"{dirTitle} authorized after validation via {methodLabel}."
            };
        }

        return locale switch
        {
            "pt-BR" => $"{dirTitle} autorizada após validação de credencial.",
            "es"    => $"{dirTitle} autorizada tras la validación de credencial.",
            _       => $"{dirTitle} authorized after credential validation."
        };
    }

    private static string BuildBlocked(string locale, string dirLower, string? methodLabel, bool isIdMethod)
    {
        // Blocked => attempt blocked. If methodLabel exists, we append it as context.
        if (!string.IsNullOrWhiteSpace(methodLabel) && isIdMethod)
        {
            return locale switch
            {
                "pt-BR" => $"Tentativa de {dirLower} bloqueada ({methodLabel}).",
                "es"    => $"Intento de {dirLower} bloqueado ({methodLabel}).",
                _       => $"{CapFirst(dirLower)} attempt blocked ({methodLabel})."
            };
        }

        return locale switch
        {
            "pt-BR" => $"Tentativa de {dirLower} bloqueada pelo controle de acesso.",
            "es"    => $"Intento de {dirLower} bloqueado por el control de acceso.",
            _       => $"{CapFirst(dirLower)} attempt blocked by access control."
        };
    }

    private static string BuildEmergency(string locale, string dirTitle)
    {
        return locale switch
        {
            "pt-BR" => $"{dirTitle} liberada por emergência.",
            "es"    => $"{dirTitle} liberada por emergencia.",
            _       => $"{dirTitle} released due to emergency."
        };
    }

    private static string BuildFreeManual(
        string locale,
        string dirTitle,
        string? methodLabel,
        bool isManualCmd,
        bool isIdentified)
    {
        // Prefer explicit command label when we have F2/F3
        if (!string.IsNullOrWhiteSpace(methodLabel) && isManualCmd)
        {
            return locale switch
            {
                "pt-BR" => $"{dirTitle} liberada manualmente via {methodLabel}.",
                "es"    => $"{dirTitle} liberada manualmente mediante {methodLabel}.",
                _       => $"{dirTitle} manually released via {methodLabel}."
            };
        }

        // Generic manual release (optionally mention identification)
        return locale switch
        {
            "pt-BR" => isIdentified
                ? $"{dirTitle} liberada manualmente para pessoa identificada."
                : $"{dirTitle} liberada manualmente sem identificação.",

            "es" => isIdentified
                ? $"{dirTitle} liberada manualmente para persona identificada."
                : $"{dirTitle} liberada manualmente sin identificación.",

            _ => isIdentified
                ? $"{dirTitle} manually released for an identified person."
                : $"{dirTitle} manually released without identification."
        };
    }

    private static string BuildFreePolicy(
        string locale,
        string dirTitle,
        string? methodLabel,
        bool isIdMethod,
        bool isIdentified)
    {
        // PolicyFree => no validation.
        // If identified + method is identification method, say "identified via ..." (NOT validated).
        if (isIdentified && !string.IsNullOrWhiteSpace(methodLabel) && isIdMethod)
        {
            return locale switch
            {
                "pt-BR" => $"{dirTitle} realizada em modo livre (política), pessoa identificada por {methodLabel}.",
                "es"    => $"{dirTitle} realizada en modo libre (política), persona identificada por {methodLabel}.",
                _       => $"{dirTitle} performed in free mode (policy), person identified via {methodLabel}."
            };
        }

        return locale switch
        {
            "pt-BR" => isIdentified
                ? $"{dirTitle} realizada em modo livre (política), com identificação."
                : $"{dirTitle} realizada em modo livre (política), sem validação.",

            "es" => isIdentified
                ? $"{dirTitle} realizada en modo libre (política), con identificación."
                : $"{dirTitle} realizada en modo libre (política), sin validación.",

            _ => isIdentified
                ? $"{dirTitle} performed in free mode (policy), with identification."
                : $"{dirTitle} performed in free mode (policy), without validation."
        };
    }

    private static string BuildFallback(string locale, string dirTitle)
    {
        return locale switch
        {
            "pt-BR" => $"{dirTitle} registrada.",
            "es"    => $"{dirTitle} registrada.",
            _       => $"{dirTitle} recorded."
        };
    }

    // -------------------------
    // Locale helpers
    // -------------------------

    private static string NormalizeLang(string? lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
            return "en";

        lang = lang.Trim();

        // Accept common variations
        if (lang.Equals("pt", StringComparison.OrdinalIgnoreCase) || lang.Equals("pt-br", StringComparison.OrdinalIgnoreCase))
            return "pt-BR";

        if (lang.Equals("es", StringComparison.OrdinalIgnoreCase) || lang.StartsWith("es-", StringComparison.OrdinalIgnoreCase))
            return "es";

        if (lang.Equals("en", StringComparison.OrdinalIgnoreCase) || lang.StartsWith("en-", StringComparison.OrdinalIgnoreCase))
            return "en";

        // Default fallback
        return "en";
    }

    private static string GetDirTitle(string locale, AccessLogicalDirectionEnum dir)
    {
        // This localizer expects Entry/Exit only.
        return locale switch
        {
            "pt-BR" => dir == AccessLogicalDirectionEnum.Entry ? "Entrada" : "Saída",
            "es"    => dir == AccessLogicalDirectionEnum.Entry ? "Entrada" : "Salida",
            _       => dir == AccessLogicalDirectionEnum.Entry ? "Entry" : "Exit"
        };
    }

    private static string GetDirLower(string locale, AccessLogicalDirectionEnum dir)
    {
        return locale switch
        {
            "pt-BR" => dir == AccessLogicalDirectionEnum.Entry ? "entrada" : "saída",
            "es"    => dir == AccessLogicalDirectionEnum.Entry ? "entrada" : "salida",
            _       => dir == AccessLogicalDirectionEnum.Entry ? "entry" : "exit"
        };
    }

    // -------------------------
    // Method helpers
    // -------------------------

    private static bool IsManualCommandMethod(AccessMethodEnum? method)
        => method == AccessMethodEnum.F2ManualEntryRelease
           || method == AccessMethodEnum.F3ManualExitRelease;

    private static bool IsIdentificationMethod(AccessMethodEnum? method)
        => method is AccessMethodEnum.Bluetooth
            or AccessMethodEnum.Card
            or AccessMethodEnum.Face
            or AccessMethodEnum.Fingerprint
            or AccessMethodEnum.Keyboard
            or AccessMethodEnum.PersonId
            or AccessMethodEnum.QRCode;

    private static string? GetMethodLabel(string locale, AccessMethodEnum? method)
    {
        if (method is null) return null;

        // We localize both identification methods and manual commands.
        return locale switch
        {
            "pt-BR" => method.Value switch
            {
                AccessMethodEnum.Card => "cartão",
                AccessMethodEnum.Face => "reconhecimento facial",
                AccessMethodEnum.Fingerprint => "biometria",
                AccessMethodEnum.QRCode => "QR Code",
                AccessMethodEnum.Bluetooth => "Bluetooth",
                AccessMethodEnum.Keyboard => "teclado",
                AccessMethodEnum.PersonId => "identificação interna",
                AccessMethodEnum.F2ManualEntryRelease => "comando manual (F2)",
                AccessMethodEnum.F3ManualExitRelease  => "comando manual (F3)",
                _ => null
            },

            "es" => method.Value switch
            {
                AccessMethodEnum.Card => "tarjeta",
                AccessMethodEnum.Face => "reconocimiento facial",
                AccessMethodEnum.Fingerprint => "huella dactilar",
                AccessMethodEnum.QRCode => "QR Code",
                AccessMethodEnum.Bluetooth => "Bluetooth",
                AccessMethodEnum.Keyboard => "teclado",
                AccessMethodEnum.PersonId => "identificación interna",
                AccessMethodEnum.F2ManualEntryRelease => "comando manual (F2)",
                AccessMethodEnum.F3ManualExitRelease  => "comando manual (F3)",
                _ => null
            },

            _ => method.Value switch
            {
                AccessMethodEnum.Card => "card",
                AccessMethodEnum.Face => "face recognition",
                AccessMethodEnum.Fingerprint => "fingerprint",
                AccessMethodEnum.QRCode => "QR Code",
                AccessMethodEnum.Bluetooth => "Bluetooth",
                AccessMethodEnum.Keyboard => "keyboard",
                AccessMethodEnum.PersonId => "internal identification",
                AccessMethodEnum.F2ManualEntryRelease => "manual command (F2)",
                AccessMethodEnum.F3ManualExitRelease  => "manual command (F3)",
                _ => null
            }
        };
    }

    private static string CapFirst(string s)
        => string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1);
}