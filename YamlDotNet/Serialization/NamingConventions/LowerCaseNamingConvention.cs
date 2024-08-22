﻿// This file is part of YamlDotNet - A .NET library for YAML.
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
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
    /// <summary>
    /// Convert the string with underscores (this_is_a_test) or hyphens (this-is-a-test) to 
    /// lower case (thisisatest).
    /// </summary>
    public sealed class LowerCaseNamingConvention : INamingConvention
    {
        private LowerCaseNamingConvention() { }

        public string Apply(string value)
        {
            return value.AsSpan().ToCamelCase().ToLower();
        }

        public string Reverse(string value)
        {
            // lower case values don't have any context as to what should be upper or not. So we only do the first character
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var span = value.AsSpan();
            var result = new string([char.ToUpperInvariant(span[0]), .. span.Slice(1)]);

            return result;

        }

        public static readonly INamingConvention Instance = new LowerCaseNamingConvention();
    }
}
