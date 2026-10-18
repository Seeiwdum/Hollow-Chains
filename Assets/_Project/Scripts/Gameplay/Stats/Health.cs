using HollowChains.Core.Events;
using UnityEngine;

namespace HollowChains.Gameplay.Stats
{
    /// <summary>
    /// Player HP (HUD shows HP only, D-002). Wraps <see cref="HealthModel"/> and publishes HP as 0–1 on an event
    /// channel for the HUD bar and the critical-HP vignette. Setup: on the Player; assign EVT_HealthChanged.
    /// </summary>
    public sealed class Health : MonoBehaviour, IDamageable
    {
        [SerializeField, Min(1)] private int _max = 10;
        [Tooltip("Raised with HP as 0–1 whenever it changes (and once on Start).")]
        [SerializeField] private FloatEventChannelSO _onHealthChanged;

        private HealthModel _model;

        public int Current => Model.Current;

        public int Max => Model.Max;

        public bool IsCritical => Model.IsCritical;

        private HealthModel Model => _model ??= new HealthModel(_max);

        private void Awake() => Model.Changed += (_, __) => Publish();

        private void Start() => Publish();

        public void TakeDamage(int amount) => Model.TakeDamage(amount);

        public void Heal(int amount) => Model.Heal(amount);

        /// <summary>Scripted beats (SC-04: HP = 1).</summary>
        public void SetTo(int value) => Model.SetTo(value);

        private void Publish()
        {
            if (_onHealthChanged != null) _onHealthChanged.Raise(Model.Normalized);
        }
    }
}
