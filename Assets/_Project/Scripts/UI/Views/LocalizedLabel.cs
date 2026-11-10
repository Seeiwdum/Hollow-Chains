using HollowChains.Core.Events;
using HollowChains.Data.Input;
using HollowChains.UI.Glyphs;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Fixed UI text (button labels, headings) from a string table; updates automatically if the locale changes.
    /// Setup: next to a TMP text; pick the table entry (usually table "UI").
    /// Key hints: if the entry contains {key:Action} tokens, assign the glyph table and EVT_InputSchemeChanged so the
    /// label shows the right glyph and updates when the player switches device.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public sealed class LocalizedLabel : MonoBehaviour
    {
        [SerializeField] private LocalizedString _text = new LocalizedString();
        [Tooltip("Optional. Resolves {key:Action} tokens in the entry.")]
        [SerializeField] private InputGlyphTableSO _glyphTable;
        [Tooltip("Optional. EVT_InputSchemeChanged; re-renders when the device changes.")]
        [SerializeField] private StringEventChannelSO _onSchemeChanged;

        private TMP_Text _label;
        private GlyphText _glyphs;

        private void OnEnable()
        {
            _label = GetComponent<TMP_Text>();
            _glyphs = new GlyphText(_glyphTable);
            if (_onSchemeChanged != null) _onSchemeChanged.Subscribe(OnSchemeChanged);
            if (!_text.IsEmpty) _text.StringChanged += OnStringChanged;
        }

        private void OnDisable()
        {
            if (_onSchemeChanged != null) _onSchemeChanged.Unsubscribe(OnSchemeChanged);
            if (!_text.IsEmpty) _text.StringChanged -= OnStringChanged;
        }

        private void OnStringChanged(string value)
        {
            _glyphs.Source = value;
            _label.text = _glyphs.Render();
        }

        private void OnSchemeChanged(string scheme)
        {
            _glyphs.SetScheme(scheme);
            if (_glyphs.Source != null) _label.text = _glyphs.Render();
        }
    }
}
