using Toletus.Pack.Core.Access.Enums;

namespace Toletus.Pack.Core.Access.Extensions;

public static class AccessIntentEnumExtensions
{
    public static bool IsUnknown(this AccessIntentEnum value)
        => value == AccessIntentEnum.Unknown;

    public static bool IsEntering(this AccessIntentEnum value)
        => value == AccessIntentEnum.Entering;

    public static bool IsExiting(this AccessIntentEnum value)
        => value == AccessIntentEnum.Exiting;

    public static bool IsKnown(this AccessIntentEnum value)
        => value is AccessIntentEnum.Entering or AccessIntentEnum.Exiting;

    /// <summary>
    /// Normaliza valores inválidos para Unknown (defensivo, útil pra deserialização).
    /// </summary>
    public static AccessIntentEnum Normalize(this AccessIntentEnum value)
        => value.IsKnown() ? value : AccessIntentEnum.Unknown;
}