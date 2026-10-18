using HollowChains.Core.Events;
using HollowChains.Core.Settings;
using TMPro;
using UnityEngine;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Shows the text carried by a string event channel; an empty string hides the view.
    /// Used for the interact prompt (EVT_InteractPrompt), the subtitle line (EVT_Subtitle) and the quest tracker (EVT_ObjectiveChanged, auto-hide).
    /// Setup: on a UI element with CanvasGroup; assign a TMP text and the channel.
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

        private float _baseFontSize;

        private float _hideAt = float.PositiveInfinity;

        protected override void Awake()
        {
            base.Awake();
            if (_text != null) _baseFontSize = _text.fontSize;
        }

        private void OnEnable()
        {
            if (_channel != null) _channel.Subscribe(OnText);
        }

        private void OnDisable()
        {
            if (_channel != null) _channel.Unsubscribe(OnText);
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
            _text.text = string.Format(_format, value);
            _hideAt = _autoHideSeconds > 0f ? Time.unscaledTime + _autoHideSeconds : float.PositiveInfinity;
            Show();
        }
    }
}
