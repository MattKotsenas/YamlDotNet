// This file is part of YamlDotNet - A .NET library for YAML.
// Copyright (c) Antoine Aubry and contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Text.RegularExpressions;
#if NET7_0_OR_GREATER
using System.Text.RegularExpressions.Generated;
#endif

namespace YamlDotNet.Serialization.Utilities
{
    /// <summary>
    /// Various string extension methods
    /// </summary>
    internal static partial class StringExtensions
    {
        private const string LowercasePattern = "([_\\-])(?<char>[a-z])";
        private static readonly Regex LowercaseRegex = CreateLowercaseRegex();

#if NET7_0_OR_GREATER
        [GeneratedRegex(LowercasePattern, RegexOptions.IgnoreCase)]
        private static partial Regex CreateLowercaseRegex();
#else
        private static Regex CreateLowercaseRegex() => new(LowercasePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
#endif

        private const string UppercasePattern = "(?<char>[A-Z])";
        private static readonly Regex UppercaseRegex = CreateUppercaseRegex();

#if NET7_0_OR_GREATER
        [GeneratedRegex(UppercasePattern)]
        private static partial Regex CreateUppercaseRegex();
#else
        private static Regex CreateUppercaseRegex() => new(UppercasePattern, RegexOptions.Compiled);
#endif

        private static string ToCamelOrPascalCase(string str, Func<char, char> firstLetterTransform)
        {
            var text = LowercaseRegex.Replace(str, match => match.Groups["char"].Value.ToUpperInvariant());
            return firstLetterTransform(text[0]) + text.Substring(1);
        }


        /// <summary>
        /// Convert the string with underscores (this_is_a_test) or hyphens (this-is-a-test) to 
        /// camel case (thisIsATest). Camel case is the same as Pascal case, except the first letter
        /// is lowercase.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>Converted string</returns>
        public static string ToCamelCase(this string str)
        {
            return ToCamelOrPascalCase(str, char.ToLowerInvariant);
        }

        /// <summary>
        /// Convert the string with underscores (this_is_a_test) or hyphens (this-is-a-test) to 
        /// pascal case (ThisIsATest). Pascal case is the same as camel case, except the first letter
        /// is uppercase.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>Converted string</returns>
        public static string ToPascalCase(this string str)
        {
            return ToCamelOrPascalCase(str, char.ToUpperInvariant);
        }

        /// <summary>
        /// Convert the string from camelcase (thisIsATest) to a hyphenated (this-is-a-test) or 
        /// underscored (this_is_a_test) string
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="separator">Separator to use between segments</param>
        /// <returns>Converted string</returns>
        public static string FromCamelCase(this string str, string separator)
        {
            // Ensure first letter is always lowercase
            str = char.ToLower(str[0]) + str.Substring(1);

            str = UppercaseRegex.Replace(str.ToCamelCase(), match => separator + match.Groups["char"].Value.ToLowerInvariant());
            return str;
        }
    }
}
