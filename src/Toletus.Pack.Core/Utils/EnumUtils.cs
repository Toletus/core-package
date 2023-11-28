using System;
using System.Linq;
using EnumsNET;

namespace Toletus.Pack.Core.Utils;

public class EnumUtils
{
    public static T Last<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Last();
    }
    
    public static string[] GetDescriptions<T>()  where T : struct, Enum
    {
        var values = Enums.GetValues(typeof(T));

        var descriptions = values.Select(c => ((T)c).AsString(EnumFormat.Description)).ToArray();

        return descriptions;
    }
}