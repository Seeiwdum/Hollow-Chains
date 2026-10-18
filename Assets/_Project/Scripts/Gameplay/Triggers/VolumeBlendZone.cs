using HollowChains.Gameplay.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>
    /// Blends a Volume in when the player walks in, holds it, then lets it fade back (SC-02: the cold village turns
    /// warm for a moment while Phineas thinks of Elena, `06` [IMPL]). Hold 0 = stay until the player leaves.
    /// Setup: trigger collider; assign a global Volume (weight 0, higher priority than the scene volume).
    /// </summary>
    public sealed class VolumeBlendZone : PlayerTriggerZone
    {
        [SerializeField] private Volume _volume;
        [SerializeField, Range(0f, 1f)] private float _weight = 1f;
        [SerializeField, Min(0.05f)] private float _fadeInSeconds = 1.5f;
        [SerializeField, Min(0f)] private float _holdSeconds = 6f;
        [SerializeField, Min(0.05f)] private float _fadeOutSeconds = 3f;

        private float _target;
        private float _releaseAt = float.PositiveInfinity;

        private void Awake()
        {
            if (_volume != null) _volume.weight = 0f;
        }

        protected override void OnPlayerEnter(PlayerController player)
        {
            _target = _weight;
            _releaseAt = _holdSeconds > 0f ? Time.time + _fadeInSeconds + _holdSeconds : float.PositiveInfinity;
        }

        protected override void OnPlayerExit(PlayerController player)
        {
            if (_holdSeconds <= 0f) _target = 0f;
        }

        private void Update()
        {
            if (_volume == null) return;
            if (Time.time >= _releaseAt)
            {
                _target = 0f;
                _releaseAt = float.PositiveInfinity;
            }
            float seconds = _target > _volume.weight ? _fadeInSeconds : _fadeOutSeconds;
            _volume.weight = Mathf.MoveTowards(_volume.weight, _target, Mathf.Max(_weight, 0.01f) / seconds * Time.deltaTime);
        }
    }
}
