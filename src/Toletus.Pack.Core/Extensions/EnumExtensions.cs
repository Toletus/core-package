using System;
using System.ComponentModel;
using System.Linq;

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

    // public static string GetDescription<T>(this T @this) where T : struct, Enum
    // {
    //     return @this.AsString(EnumFormat.Description);
    // }

    public static string GetDescription(this Enum value)
    {
        if (value == null) return null;

        var field = value.GetType().GetField(value.ToString());

        var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;

        return attribute == null ? value.ToString() : attribute.Description;
    }

    public static T GetValueFromDescription<T>(this string description)
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException();
        foreach (var field in type.GetFields())
        {
            var attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T)field.GetValue(null);
            }
        }

        return default(T);
    }
}