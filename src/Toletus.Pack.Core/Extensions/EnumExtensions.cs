using System;
using EnumsNET;

namespace Toletus.Pack.Core.Extensions;

public static class EnumExtensions
{
    public static bool In(this Enum @this, params Enum[] values)
    {
        return Array.IndexOf(values, @this) != -1;
    }

    public static bool NotIn(this Enum @this, params Enum[] values)
    {
        return Array.IndexOf(values, @this) == -1;
    }

    public static T Parse<T>(this Enum @this) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), @this.ToString());
    }

    public static string[] GetNames(this Enum @this)
    {
        return Enum.GetNames(@this.GetType());
    }
    
    public static string? GetDescription<T>(this T @this) where T : struct, Enum
    {
        return @this.AsString(EnumFormat.Description);
    }
}