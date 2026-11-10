using HollowChains.Core.Events;
using HollowChains.Core.Settings;
using HollowChains.Data.Input;
using HollowChains.UI.Glyphs;
using TMPro;
using UnityEngine;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Shows the text carried by a string event channel; an empty string hides the view.
    /// Used for the interact prompt (EVT_InteractPrompt), the subtitle line (EVT_Subtitle) and the quest tracker (EVT_ObjectiveChanged, auto-hide).
    /// Setup: on a UI element with CanvasGroup; assign a TMP text and the channel.
    /// Key hints: the format and the incoming text may contain {key:Action} tokens (e.g. "{key:Interact}  {0}"). Assign the
    /// glyph table and EVT_InputSchemeChanged to resolve them for the current device and re-render when it changes.
    /// Note: the format uses {0} as a plain placeholder (not string.Format), so token braces are safe.
    /// </summary>
    public sealed class StringChannelTextView : UIView
    {
        [SerializeField] private StringEventChannelSO _channel;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _format = "{0}";
        [Tooltip("0 = stay visible until an empty string arrives.")]
        [SerializeField, Min(0f)] private float _autoHideSeconds;
        [Tooltip("Scale the font with the player's subtitle-size setting (subtitle line only).")]
        [SerializeField] private bool _useSubtitleScale;
        [Tooltip("Optional. Resolves {key:Action} tokens; leave empty for text without key hints.")]
        [SerializeField] private InputGlyphTableSO _glyphTable;
        [Tooltip("Optional. EVT_InputSchemeChanged; re-renders the visible text when the device changes.")]
        [SerializeField] private StringEventChannelSO _onSchemeChanged;

        private float _baseFontSize;

        private float _hideAt = float.PositiveInfinity;

        private GlyphText _glyphs;

        protected override void Awake()
        {
            base.Awake();
            if (_text != null) _baseFontSize = _text.fontSize;
            _glyphs = new GlyphText(_glyphTable);
        }

        private void OnEnable()
        {
            if (_channel != null) _channel.Subscribe(OnText);
            if (_onSchemeChanged != null) _onSchemeChanged.Subscribe(OnSchemeChanged);
        }

        private void OnDisable()
        {
            if (_channel != null) _channel.Unsubscribe(OnText);
            if (_onSchemeChanged != null) _onSchemeChanged.Unsubscribe(OnSchemeChanged);
        }

        private void OnSchemeChanged(string scheme)
        {
            _glyphs.SetScheme(scheme);
            if (IsVisible && !string.IsNullOrEmpty(_glyphs.Source)) _text.text = _glyphs.Render();
        }

        private void Update()
        {
            if (IsVisible && Time.unscaledTime >= _hideAt) Hide();
        }

        private void OnText(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Hide();
                return;
            }
            if (_useSubtitleScale) _text.fontSize = _baseFontSize * GameSettings.Current.SubtitleScale;
            _glyphs.Source = _format.Replace("{0}", value);
            _text.text = _glyphs.Render();
            _hideAt = _autoHideSeconds > 0f ? Time.unscaledTime + _autoHideSeconds : float.PositiveInfinity;
            Show();
        }
    }
}
