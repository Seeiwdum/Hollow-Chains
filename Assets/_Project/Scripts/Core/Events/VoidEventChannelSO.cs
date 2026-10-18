using System;
using UnityEngine;

namespace HollowChains.Core.Events
{
    /// <summary>
    /// Event channel without a payload, e.g. "bucket picked up" or "checkpoint reached".
    /// Listeners subscribe in OnEnable and unsubscribe in OnDisable.
    /// </summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Events/Void Event Channel", fileName = "EVT_NewVoid")]
    public sealed class VoidEventChannelSO : EventChannelBaseSO
    {
        private event Action Raised;

        public void Raise() => Raised?.Invoke();

        public void Subscribe(Action listener) => Raised += listener;

        public void Unsubscribe(Action listener) => Raised -= listener;
    }
}
