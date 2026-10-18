using HollowChains.Core.Events;
using TMPro;
using UnityEngine;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Shows the text carried by a string event channel; an empty string hides the view.
    /// Used for the interact prompt (EVT_InteractPrompt) and for inspect thoughts (EVT_Thought, auto-hide).
    /// Setup: on a UI element with CanvasGroup; assign a TMP text and the channel.
    /// </summary>
    public sealed class StringChannelTextView : UIView
    {
        [SerializeField] private StringEventChannelSO _channel;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _format = "{0}";
        [Tooltip("0 = stay visible until an empty string arrives.")]
        [SerializeField, Min(0f)] private float _autoHideSeconds;

        private float _hideAt = float.PositiveInfinity;

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
            _text.text = string.Format(_format, value);
            _hideAt = _autoHideSeconds > 0f ? Time.unscaledTime + _autoHideSeconds : float.PositiveInfinity;
            Show();
        }
    }
}
