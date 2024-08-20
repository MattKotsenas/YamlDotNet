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
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
    public readonly struct AliasEventInfo
    {
        public AliasEventInfo(IObjectDescriptor source, AnchorName alias)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));

            if (alias.IsEmpty)
            {
                throw new ArgumentNullException(nameof(alias));
            }
            Alias = alias;
        }

        public IObjectDescriptor Source { get; }
        public AnchorName Alias { get; }
        public bool NeedsExpansion { get; init; }
    }

    internal interface IObjectEventInfo
    {
        public IObjectDescriptor Source { get; }
        public AnchorName Anchor { get; init; }
        public TagName Tag { get; init; }
    }

    public readonly struct ScalarEventInfo : IObjectEventInfo
    {
        public ScalarEventInfo(IObjectDescriptor source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Style = source.ScalarStyle;
            RenderedValue = string.Empty;
        }

        public IObjectDescriptor Source { get; }
        public AnchorName Anchor { get; init; }
        public TagName Tag { get; init; }
        public string RenderedValue { get; init; }
        public ScalarStyle Style { get; init; }
        public bool IsPlainImplicit { get; init; }
        public bool IsQuotedImplicit { get; init; }
    }

    public readonly struct MappingStartEventInfo : IObjectEventInfo
    {
        public MappingStartEventInfo(IObjectDescriptor source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public IObjectDescriptor Source { get; }
        public AnchorName Anchor { get; init; }
        public TagName Tag { get; init; }
        public bool IsImplicit { get; init; }
        public MappingStyle Style { get; init; }
    }

    public readonly struct MappingEndEventInfo
    {
        public MappingEndEventInfo(IObjectDescriptor source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public IObjectDescriptor Source { get; }
    }

    public readonly struct SequenceStartEventInfo : IObjectEventInfo
    {
        public SequenceStartEventInfo(IObjectDescriptor source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public IObjectDescriptor Source { get; }
        public AnchorName Anchor { get; init; }
        public TagName Tag { get; init; }
        public bool IsImplicit { get; init; }
        public SequenceStyle Style { get; init; }
    }

    public readonly struct SequenceEndEventInfo
    {
        public SequenceEndEventInfo(IObjectDescriptor source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public IObjectDescriptor Source { get; }
    }
}
