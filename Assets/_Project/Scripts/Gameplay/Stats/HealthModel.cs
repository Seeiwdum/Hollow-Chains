using System;

namespace HollowChains.Gameplay.Stats
{
    /// <summary>
    /// HP rules without Unity types (unit-testable): clamped between 0 and Max, change notifications.
    /// SC-04 sets HP to exactly 1 (CANON), SC-05 heals it back through the Soul Drain.
    /// </summary>
    public sealed class HealthModel
    {
        public HealthModel(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            Max = max;
            Current = max;
        }

        public int Max { get; }

        public int Current { get; private set; }

        public float Normalized => (float)Current / Max;

        public bool IsCritical => Current <= 1;

        /// <summary>(current, max) after every change.</summary>
        public event Action<int, int> Changed;

        public void TakeDamage(int amount) => SetTo(Current - Math.Max(0, amount));

        public void Heal(int amount) => SetTo(Current + Math.Max(0, amount));

        public void SetTo(int value)
        {
            int clamped = Math.Clamp(value, 0, Max);
            if (clamped == Current) return;
            Current = clamped;
            Changed?.Invoke(Current, Max);
        }
    }
}
