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
using System.Buffers;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Helpers;

namespace YamlDotNet.Serialization.Utilities
{
    /// <summary>
    /// Various string extension methods
    /// </summary>
    internal static class StringExtensions
    {
#if NET8_0_OR_GREATER
        private static readonly SearchValues<char> Separators = SearchValues.Create(['-', '_']);
#else
        private static readonly char[] Separators = ['-', '_'];
#endif

        private static string ToCamelOrPascalCase(ReadOnlySpan<char> span, Func<char, char> firstLetterTransform)
        {
            //using var wrapper = StringBuilderPool.Rent();
            var builder = new ValueStringBuilder(span.Length); //wrapper.Builder;

            for (var i = 0; i < span.Length; i++)
            {
                if (Separators.Contains(span[i]))
                {
                    if (i + 1 < span.Length)
                    {
                        builder.Append(char.ToUpperInvariant(span[i + 1]));
                        i++;
                    }
                }
                else
                {
                    builder.Append(span[i]);
                }
            }

            builder[0] = firstLetterTransform(builder[0]);

            return builder.ToString();

            //var text = Regex.Replace(str, "([_\\-])(?<char>[a-z])", match => match.Groups["char"].Value.ToUpperInvariant(), RegexOptions.IgnoreCase);
            //return firstLetterTransform(text.AsSpan()[0]) + text.Substring(1);
        }


        /// <summary>
        /// Convert the string with underscores (this_is_a_test) or hyphens (this-is-a-test) to 
        /// camel case (thisIsATest). Camel case is the same as Pascal case, except the first letter
        /// is lowercase.
        /// </summary>
        /// <param name="span">String to convert</param>
        /// <returns>Converted string</returns>
        public static string ToCamelCase(this ReadOnlySpan<char> span)
        {
            return ToCamelOrPascalCase(span, char.ToLowerInvariant);
        }

        /// <summary>
        /// Convert the string with underscores (this_is_a_test) or hyphens (this-is-a-test) to 
        /// pascal case (ThisIsATest). Pascal case is the same as camel case, except the first letter
        /// is uppercase.
        /// </summary>
        /// <param name="span">String to convert</param>
        /// <returns>Converted string</returns>
        public static string ToPascalCase(this ReadOnlySpan<char> span)
        {
            return ToCamelOrPascalCase(span, char.ToUpperInvariant);
        }

        /// <summary>
        /// Convert the string from camelcase (thisIsATest) to a hyphenated (this-is-a-test) or 
        /// underscored (this_is_a_test) string
        /// </summary>
        /// <param name="span">String to convert</param>
        /// <param name="separator">Separator to use between segments</param>
        /// <returns>Converted string</returns>
        public static string FromCamelCase(this ReadOnlySpan<char> span, char separator)
        {
            //using var wrapper = StringBuilderPool.Rent();
            var builder = new ValueStringBuilder(span.Length);

            // Ensure first letter is always lowercase
            builder.Append(char.ToLower(span[0]));

            span = span.Slice(1);

            for (var i = 0; i < span.Length; i++)
            {
                if (Separators.Contains(span[i]))
                {
                    builder.Append(separator);
                    if (i + 1 < span.Length)
                    {
                        builder.Append(char.ToLowerInvariant(span[i + 1]));
                        i++;
                    }
                }
                else if (char.IsUpper(span[i]))
                {
                    builder.Append(separator);
                    builder.Append(char.ToLowerInvariant(span[i]));
                }
                else
                {
                    builder.Append(span[i]);
                }
            }

            //foreach (var c in span)
            //{
            //    if (char.IsUpper(c))
            //    {
            //        builder.Append(separator);
            //        builder.Append(char.ToLowerInvariant(c));
            //    }
            //    else
            //    {
            //        builder.Append(c);
            //    }
            //}

            return builder.ToString();

            //// Ensure first letter is always lowercase
            //str = char.ToLower(str[0]) + str.Substring(1);

            //str = Regex.Replace(str.ToCamelCase(), "(?<char>[A-Z])", match => separator + match.Groups["char"].Value.ToLowerInvariant());
            //return str;
        }
    }
}
