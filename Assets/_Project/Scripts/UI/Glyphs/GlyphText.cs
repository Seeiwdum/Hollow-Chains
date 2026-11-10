using HollowChains.Core.Input;
using HollowChains.Core.Services;
using HollowChains.Data.Input;

namespace HollowChains.UI.Glyphs
{
    /// <summary>
    /// Helper for views that show key hints: keeps the unresolved source string and renders it for the current scheme.
    /// Without a glyph table it passes the text through unchanged. Scheme comes from <see cref="IInputSchemeService"/>
    /// when registered, else the last value pushed by <see cref="SetScheme"/>, else "kbm".
    /// </summary>
    public sealed class GlyphText
    {
        private readonly GlyphResolver _resolver;
        private string _pushedScheme;

        public GlyphText(InputGlyphTableSO table)
        {
            _resolver = table != null ? new GlyphResolver(table) : null;
        }

        /// <summary>The text as authored, with {key:...} tokens.</summary>
        public string Source { get; set; }

        public void SetScheme(string scheme) => _pushedScheme = scheme;

        public string Render()
        {
            if (_resolver == null) return Source;
            string scheme = ServiceLocator.TryGet<IInputSchemeService>(out var service)
                ? service.CurrentScheme
                : _pushedScheme ?? InputSchemes.KeyboardMouse;
            return _resolver.Resolve(Source, scheme);
        }
    }
}
