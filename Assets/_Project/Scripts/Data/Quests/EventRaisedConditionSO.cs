using System;
using HollowChains.Core.Events;
using UnityEngine;

namespace HollowChains.Data.Quests
{
    /// <summary>Phase completes when a void event channel is raised (bucket picked up, scream heard, gate reached…).</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Quests/Condition: Event Raised", fileName = "COND_NewEvent")]
    public sealed class EventRaisedConditionSO : PhaseConditionSO
    {
        [SerializeField] private VoidEventChannelSO _event;

        public VoidEventChannelSO Event => _event;

        public override IConditionWatcher CreateWatcher() => new Watcher(_event);

        private sealed class Watcher : IConditionWatcher
        {
            private readonly VoidEventChannelSO _channel;
            private Action _onMet;

            public Watcher(VoidEventChannelSO channel) => _channel = channel;

            public void Start(Action onMet)
            {
                _onMet = onMet;
                if (_channel != null) _channel.Subscribe(OnRaised);
            }

            public void Dispose()
            {
                if (_channel != null) _channel.Unsubscribe(OnRaised);
                _onMet = null;
            }

            private void OnRaised()
            {
                var callback = _onMet;
                Dispose(); // fire once
                callback?.Invoke();
            }
        }
    }
}
