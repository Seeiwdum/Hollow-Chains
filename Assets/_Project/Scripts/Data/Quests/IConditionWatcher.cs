using System;

namespace HollowChains.Data.Quests
{
    /// <summary>
    /// Runtime watcher created by a <see cref="PhaseConditionSO"/> for one quest phase. Keeps the listening state
    /// out of the shared ScriptableObject asset.
    /// </summary>
    public interface IConditionWatcher : IDisposable
    {
        /// <summary>Starts watching; <paramref name="onMet"/> is invoked once when the condition is satisfied.</summary>
        void Start(Action onMet);
    }
}
