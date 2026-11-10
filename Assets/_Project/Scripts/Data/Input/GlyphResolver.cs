using System;
using System.Text.RegularExpressions;

namespace HollowChains.Data.Input
{
    /// <summary>
    /// Replaces {key:Action} tokens in a string with the glyph for a scheme. Unknown actions keep their token so
    /// a missing table row is visible instead of silently blank. Plain C#, no Unity dependency beyond the lookup.
    /// </summary>
    public sealed class GlyphResolver
    {
        private static readonly Regex Token = new Regex(@"\{key:([A-Za-z0-9_]+)\}", RegexOptions.Compiled);

        private readonly IGlyphLookup _lookup;

        public GlyphResolver(IGlyphLookup lookup)
        {
            _lookup = lookup ?? throw new ArgumentNullException(nameof(lookup));
        }

        public string Resolve(string source, string scheme)
        {
            if (string.IsNullOrEmpty(source) || source.IndexOf("{key:", StringComparison.Ordinal) < 0) return source;
            return Token.Replace(source, m => _lookup.TryGetGlyph(m.Groups[1].Value, scheme, out var glyph) ? glyph : m.Value);
        }
    }
}
