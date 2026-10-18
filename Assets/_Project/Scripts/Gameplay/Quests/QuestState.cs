using System;

namespace HollowChains.Gameplay.Quests
{
    /// <summary>Serializable runtime progress of one quest — this is what gets saved, never the QuestSO.</summary>
    [Serializable]
    public sealed class QuestState
    {
        public string QuestId;
        public int PhaseIndex;
        public bool IsComplete;

        public QuestState() { }

        public QuestState(string questId)
        {
            QuestId = questId;
        }
    }
}
