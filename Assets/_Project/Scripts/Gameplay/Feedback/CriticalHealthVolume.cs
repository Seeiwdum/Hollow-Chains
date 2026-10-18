using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.Rendering;

namespace HollowChains.Gameplay.Feedback
{
    /// <summary>
    /// Blends the red "critical" Volume in when HP is at or below the threshold (SC-04: HP = 1, Art Bible §4).
    /// Setup: in _Boot on a global Volume using VP_Critical (weight 0); assign EVT_HealthChanged.
    /// </summary>
    [RequireComponent(typeof(Volume))]
    public sealed class CriticalHealthVolume : MonoBehaviour
    {
        [SerializeField] private FloatEventChannelSO _onHealthChanged;
        [Tooltip("HP fraction at or below which the screen turns critical (1 of 10 HP = 0.1).")]
        [SerializeField, Range(0f, 1f)] private float _threshold = 0.15f;
        [SerializeField, Min(0.01f)] private float _blendSpeed = 3f;

        private Volume _volume;
        private float _target;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.weight = 0f;
        }

        private void OnEnable()
        {
            if (_onHealthChanged != null) _onHealthChanged.Subscribe(OnHealth);
        }

        private void OnDisable()
        {
            if (_onHealthChanged != null) _onHealthChanged.Unsubscribe(OnHealth);
        }

        private void Update() =>
            _volume.weight = Mathf.MoveTowards(_volume.weight, _target, _blendSpeed * Time.unscaledDeltaTime);

        private void OnHealth(float normalized) => _target = normalized > 0f && normalized <= _threshold ? 1f : 0f;
    }
}
