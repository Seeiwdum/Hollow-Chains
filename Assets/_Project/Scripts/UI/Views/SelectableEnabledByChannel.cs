using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Enables/disables a button from a bool channel (title "Continue" ← EVT_SaveAvailable). Dimmed while disabled.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public sealed class SelectableEnabledByChannel : MonoBehaviour
    {
        [SerializeField] private BoolEventChannelSO _channel;
        [SerializeField] private bool _defaultEnabled;

        private Selectable _selectable;

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _selectable.interactable = _defaultEnabled;
        }

        private void OnEnable()
        {
            if (_channel != null) _channel.Subscribe(Apply);
        }

        private void OnDisable()
        {
            if (_channel != null) _channel.Unsubscribe(Apply);
        }

        private void Apply(bool enabled) => _selectable.interactable = enabled;
    }
}
