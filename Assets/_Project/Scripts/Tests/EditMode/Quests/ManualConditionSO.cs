using System;
using HollowChains.Data.Quests;

namespace HollowChains.Tests.Quests
{
    /// <summary>Test-only condition completed by calling <see cref="Fire"/>.</summary>
    public sealed class ManualConditionSO : PhaseConditionSO
    {
        private event Action Fired;

        public int ActiveWatchers { get; private set; }

        public void Fire() => Fired?.Invoke();

        public override IConditionWatcher CreateWatcher() => new Watcher(this);

        private sealed class Watcher : IConditionWatcher
        {
            private readonly ManualConditionSO _owner;
            private Action _onMet;

            public Watcher(ManualConditionSO owner) => _owner = owner;

            public void Start(Action onMet)
            {
                _onMet = onMet;
                _owner.Fired += OnFired;
                _owner.ActiveWatchers++;
            }

            public void Dispose()
            {
                if (_onMet == null) return;
                _owner.Fired -= OnFired;
                _owner.ActiveWatchers--;
                _onMet = null;
            }

            private void OnFired()
            {
                var callback = _onMet;
                Dispose();
                callback?.Invoke();
            }
        }
    }
}
