using System;

namespace HollowChains.Core.Events
{
    /// <summary>
    /// Event channel carrying a value of type <typeparamref name="T"/>.
    /// Subclass it once per payload type (Unity cannot create assets of open generic types).
    /// </summary>
    public abstract class EventChannelSO<T> : EventChannelBaseSO
    {
        private event Action<T> Raised;

        public void Raise(T value) => Raised?.Invoke(value);

        public void Subscribe(Action<T> listener) => Raised += listener;

        public void Unsubscribe(Action<T> listener) => Raised -= listener;
    }
}
