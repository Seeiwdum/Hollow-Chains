using HollowChains.Core.Services;
using HollowChains.Data.Quests;

namespace HollowChains.Gameplay.Quests
{
    /// <summary>Global quest progress (lives in _Boot so it survives scene loads).</summary>
    public interface IQuestService : IService
    {
        void StartQuest(QuestSO quest);

        /// <summary>Forgets all progress and restarts the auto-start quests (New Game).</summary>
        void ResetAll();

        /// <summary>Progress of a quest by ID, or null if it was never started.</summary>
        QuestState GetState(string questId);

        /// <summary>Current phase of a running quest, or null.</summary>
        QuestPhaseSO GetCurrentPhase(string questId);
    }
}
