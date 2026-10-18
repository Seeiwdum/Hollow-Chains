using UnityEngine;

namespace HollowChains.Data.Quests
{
    /// <summary>
    /// "When is this quest phase done?" Subclass once per kind of condition (event raised, item held, …) —
    /// new condition types never require changing the quest runtime (Open/Closed).
    /// </summary>
    public abstract class PhaseConditionSO : ScriptableObject
    {
        public abstract IConditionWatcher CreateWatcher();
    }
}
