using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.Rendering;

namespace HollowChains.Gameplay.Feedback
{
    /// <summary>
    /// Blends the red "critical" Volume in when HP is at or below the threshold (SC-04: HP = 1, Art Bible §4).
    /// Clears instantly on the title screen (the player that was at HP 1 is gone, so no new HP event comes).
    /// Setup: in _Boot on a global Volume using VP_Critical (weight 0); assign EVT_HealthChanged and EVT_GameStateChanged.
    /// </summary>
    [RequireComponent(typeof(Volume))]
    public sealed class CriticalHealthVolume : MonoBehaviour
    {
        [SerializeField] private FloatEventChannelSO _onHealthChanged;
        [SerializeField] private StringEventChannelSO _onGameStateChanged;
        [SerializeField] private string _clearInState = "Title";
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
            if (_onGameStateChanged != null) _onGameStateChanged.Subscribe(OnGameState);
        }

        private void OnDisable()
        {
            if (_onHealthChanged != null) _onHealthChanged.Unsubscribe(OnHealth);
            if (_onGameStateChanged != null) _onGameStateChanged.Unsubscribe(OnGameState);
        }

        private void OnGameState(string state)
        {
            if (state != _clearInState) return;
            _target = 0f;
            _volume.weight = 0f;
        }

        private void Update() =>
            _volume.weight = Mathf.MoveTowards(_volume.weight, _target, _blendSpeed * Time.unscaledDeltaTime);

        private void OnHealth(float normalized) => _target = normalized > 0f && normalized <= _threshold ? 1f : 0f;
    }
}
