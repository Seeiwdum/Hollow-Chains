using System.Collections.Generic;
using HollowChains.Core.Events;
using HollowChains.Data.Quests;
using UnityEngine;

namespace HollowChains.Gameplay.Quests
{
    /// <summary>
    /// <see cref="IQuestService"/> in the _Boot scene. Runs a <see cref="QuestRunner"/> per started quest and
    /// publishes changes on event channels for the HUD tracker (objective text) and other listeners (phase ID).
    /// Setup: on the "Game" object in _Boot; registered by GameBootstrapper; list Q001 in Auto Start for the demo.
    /// </summary>
    public sealed class QuestService : MonoBehaviour, IQuestService
    {
        [Tooltip("Quests started when the game boots (demo: Q001).")]
        [SerializeField] private List<QuestSO> _autoStart = new List<QuestSO>();
        [Tooltip("Raised with the new objective text (empty when a quest completes).")]
        [SerializeField] private StringEventChannelSO _onObjectiveChanged;
        [Tooltip("Raised with the new phase ID, e.g. \"Q001-P2\".")]
        [SerializeField] private StringEventChannelSO _onPhaseChanged;
        [Tooltip("Raised with the quest ID when a quest completes.")]
        [SerializeField] private StringEventChannelSO _onQuestCompleted;

        private readonly Dictionary<string, QuestRunner> _runners = new Dictionary<string, QuestRunner>();

        private void Start()
        {
            foreach (var quest in _autoStart)
                if (quest != null) StartQuest(quest);
        }

        private void OnDestroy()
        {
            foreach (var runner in _runners.Values) runner.Dispose();
            _runners.Clear();
        }

        public void StartQuest(QuestSO quest)
        {
            if (quest == null || _runners.ContainsKey(quest.Id)) return;
            var runner = new QuestRunner(quest);
            runner.PhaseStarted += OnPhaseStarted;
            runner.Completed += OnCompleted;
            _runners.Add(quest.Id, runner);
            runner.Begin();
        }

        public QuestState GetState(string questId) =>
            _runners.TryGetValue(questId, out var runner) ? runner.State : null;

        public QuestPhaseSO GetCurrentPhase(string questId) =>
            _runners.TryGetValue(questId, out var runner) ? runner.CurrentPhase : null;

        private void OnPhaseStarted(QuestPhaseSO phase)
        {
            if (_onPhaseChanged != null) _onPhaseChanged.Raise(phase.Id);
            if (_onObjectiveChanged != null) _onObjectiveChanged.Raise(phase.Objective);
        }

        private void OnCompleted(QuestSO quest)
        {
            if (_onObjectiveChanged != null) _onObjectiveChanged.Raise(string.Empty);
            if (_onQuestCompleted != null) _onQuestCompleted.Raise(quest.Id);
        }
    }
}
