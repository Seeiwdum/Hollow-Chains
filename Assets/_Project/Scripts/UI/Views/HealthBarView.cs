using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// The only HUD gauge in the demo (D-002): a thin HP line, no numbers. Eases toward the new value and turns
    /// blood-red when critical. Setup: on the HP bar root; assign a Filled Image and EVT_HealthChanged (0–1).
    /// Visibility is driven by the parent HUD GameStateView, so this view never hides itself.
    /// </summary>
    public sealed class HealthBarView : MonoBehaviour
    {
        [SerializeField] private FloatEventChannelSO _onHealthChanged;
        [SerializeField] private Image _fill;
        [SerializeField] private Color _normal = new Color(0.95f, 0.89f, 0.77f, 0.9f);
        [SerializeField] private Color _critical = new Color(0.48f, 0.06f, 0.08f, 1f);
        [SerializeField, Range(0f, 1f)] private float _criticalBelow = 0.15f;
        [SerializeField, Min(0.1f)] private float _easeSpeed = 2.5f;

        private float _target = 1f;

        private void OnEnable()
        {
            if (_onHealthChanged != null) _onHealthChanged.Subscribe(OnHealth);
        }

        private void OnDisable()
        {
            if (_onHealthChanged != null) _onHealthChanged.Unsubscribe(OnHealth);
        }

        private void Update()
        {
            if (_fill == null) return;
            _fill.fillAmount = Mathf.MoveTowards(_fill.fillAmount, _target, _easeSpeed * Time.unscaledDeltaTime);
            _fill.color = _target <= _criticalBelow ? _critical : _normal;
        }

        private void OnHealth(float normalized) => _target = Mathf.Clamp01(normalized);
    }
}
