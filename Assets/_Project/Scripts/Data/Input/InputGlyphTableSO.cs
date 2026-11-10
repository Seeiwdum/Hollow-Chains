using System;
using System.Collections.Generic;
using UnityEngine;

namespace HollowChains.Data.Input
{
    /// <summary>
    /// Data table: (action, scheme) to text glyph, e.g. (Interact, kbm) = "[E]", (Interact, gamepad) = "(Y)".
    /// Adding an action or scheme means adding rows here; no code changes. Missing rows fall back to the fallback scheme.
    /// Create via Assets/Create/Hollow Chains/Input/Glyph Table; one asset (GLYPHS_Default) is shared by all views.
    /// </summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Input/Glyph Table", fileName = "GLYPHS_Default")]
    public sealed class InputGlyphTableSO : ScriptableObject, IGlyphLookup
    {
        [Serializable]
        public struct Entry
        {
            [Tooltip("Token name, e.g. Interact (used as {key:Interact}). Case-insensitive.")]
            public string Action;
            [Tooltip("Scheme ID: kbm or gamepad.")]
            public string Scheme;
            [Tooltip("Text shown, e.g. [E] or (Y).")]
            public string Glyph;
        }

        [SerializeField] private string _fallbackScheme = "kbm";
        [SerializeField] private List<Entry> _entries = new List<Entry>();

        private Dictionary<string, string> _map;

        public bool TryGetGlyph(string action, string scheme, out string glyph)
        {
            if (_map == null) Build();
            if (_map.TryGetValue(Key(action, scheme), out glyph)) return true;
            return _map.TryGetValue(Key(action, _fallbackScheme), out glyph);
        }

        private void OnValidate() => _map = null;

        private void OnEnable() => _map = null;

        private void Build()
        {
            _map = new Dictionary<string, string>();
            foreach (var e in _entries)
                if (!string.IsNullOrEmpty(e.Action) && !string.IsNullOrEmpty(e.Scheme)) _map[Key(e.Action, e.Scheme)] = e.Glyph;
        }

        private static string Key(string action, string scheme) =>
            (action ?? string.Empty).ToLowerInvariant() + "|" + (scheme ?? string.Empty).ToLowerInvariant();
    }
}
