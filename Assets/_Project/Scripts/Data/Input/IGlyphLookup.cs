namespace HollowChains.Data.Input
{
    /// <summary>Finds the on-screen text for an action under an input scheme (implemented by the glyph table asset).</summary>
    public interface IGlyphLookup
    {
        bool TryGetGlyph(string action, string scheme, out string glyph);
    }
}
