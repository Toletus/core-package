using System.Text.RegularExpressions;

namespace Toletus.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Length <= maxLength ? input : input.Substring(0, maxLength);
        }

        private static readonly Regex WhitespaceRegex = new(@"\s+");
        
        public static string ReplaceWhitespace(this string input, string replacement)
        {
            return WhitespaceRegex.Replace(input, replacement);
        }

        public static string RemoveWhitespace(this string input)
        {
            return WhitespaceRegex.Replace(input, string.Empty);
        }

        public static string RemoveDiacritics(this string input)
        {
            var tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(input);
            var asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

            return asciiStr;
        }
    }
}
