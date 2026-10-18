using HollowChains.Core.Events;
using UnityEngine;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Briefly shows a fixed element when a string channel fires (payload ignored) — e.g. the "saved" indicator on
    /// EVT_CheckpointSaved. Child text should use LocalizedLabel.
    /// </summary>
    public sealed class FlashOnEventView : UIView
    {
        [SerializeField] private StringEventChannelSO _channel;
        [SerializeField, Min(0.1f)] private float _seconds = 2.5f;

        private float _hideAt = float.PositiveInfinity;

        private void OnEnable()
        {
            if (_channel != null) _channel.Subscribe(OnEvent);
        }

        private void OnDisable()
        {
            if (_channel != null) _channel.Unsubscribe(OnEvent);
        }

        private void Update()
        {
            if (IsVisible && Time.unscaledTime >= _hideAt) Hide();
        }

        private void OnEvent(string _)
        {
            _hideAt = Time.unscaledTime + _seconds;
            Show();
        }
    }
}
