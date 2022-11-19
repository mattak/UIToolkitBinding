using System.Text.RegularExpressions;

namespace UIToolkitBinding.EditorRuntime
{
    public static class StringExtension
    {
        public static string ToUpperCamel(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (text.Length == 1) return text.ToUpperInvariant();

            var first = text[0].ToString().ToUpperInvariant();
            var last = Regex.Replace(text.Substring(1), "_([a-z])", m => m.Groups[1].Value.ToUpperInvariant());
            return first + last;
        }
    }
}