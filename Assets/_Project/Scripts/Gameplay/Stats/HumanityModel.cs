using System;

namespace HollowChains.Gameplay.Stats
{
    /// <summary>
    /// Humanity 0–100 (`03` §2): drops when demon power is used (SC-05 Soul Drain). Data only in the demo — never shown
    /// on the HUD (D-002). Plain class → unit-testable.
    /// </summary>
    public sealed class HumanityModel
    {
        public const int Min = 0;
        public const int Max = 100;

        public HumanityModel(int value = Max) => Value = Math.Clamp(value, Min, Max);

        public int Value { get; private set; }

        public event Action<int> Changed;

        public void Change(int delta) => Set(Value + delta);

        public void Set(int value)
        {
            int clamped = Math.Clamp(value, Min, Max);
            if (clamped == Value) return;
            Value = clamped;
            Changed?.Invoke(Value);
        }
    }
}
