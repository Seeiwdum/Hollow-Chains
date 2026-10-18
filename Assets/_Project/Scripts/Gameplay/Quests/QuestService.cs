using System;
using System.Collections.Generic;
using HollowChains.Core.Events;
using HollowChains.Core.Save;
using HollowChains.Core.Services;
using HollowChains.Data.Quests;
using UnityEngine;

namespace HollowChains.Gameplay.Quests
{
    /// <summary>
    /// <see cref="IQuestService"/> + <see cref="ISaveable"/> ("quests") in the _Boot scene. Runs a
    /// <see cref="QuestRunner"/> per started quest and publishes changes on event channels for the HUD tracker
    /// (objective text) and other listeners (phase ID).
    /// Setup: on the "Game" object in _Boot; registered by GameBootstrapper; list Q001 in Auto Start for the demo.
    /// </summary>
    public sealed class QuestService : MonoBehaviour, IQuestService, ISaveable
    {
        [Tooltip("Quests started when the game boots (demo: Q001). Also the lookup used when loading a save.")]
        [SerializeField] private List<QuestSO> _autoStart = new List<QuestSO>();
        [Tooltip("Raised with the new objective text (empty when a quest completes).")]
        [SerializeField] private StringEventChannelSO _onObjectiveChanged;
        [Tooltip("Raised with the new phase ID, e.g. \"Q001-P2\".")]
        [SerializeField] private StringEventChannelSO _onPhaseChanged;
        [Tooltip("Raised with the quest ID when a quest completes.")]
        [SerializeField] private StringEventChannelSO _onQuestCompleted;

        private readonly Dictionary<string, QuestRunner> _runners = new Dictionary<string, QuestRunner>();

        public string SaveKey => "quests";

        [Serializable]
        private sealed class SavedQuests
        {
            public List<QuestState> States = new List<QuestState>();
        }

        private void Start()
        {
            if (ServiceLocator.TryGet<ISaveService>(out var save)) save.Register(this);
            StartAutoQuests();
        }

        private void OnDestroy()
        {
            if (ServiceLocator.TryGet<ISaveService>(out var save)) save.Unregister(this);
            DisposeRunners();
        }

        public void ResetAll()
        {
            DisposeRunners();
            StartAutoQuests();
        }

        public void StartQuest(QuestSO quest)
        {
            if (quest == null || _runners.ContainsKey(quest.Id)) return;
            Run(new QuestRunner(quest));
        }

        public QuestState GetState(string questId) =>
            _runners.TryGetValue(questId, out var runner) ? runner.State : null;

        public QuestPhaseSO GetCurrentPhase(string questId) =>
            _runners.TryGetValue(questId, out var runner) ? runner.CurrentPhase : null;

        public string CaptureState()
        {
            var saved = new SavedQuests();
            foreach (var runner in _runners.Values) saved.States.Add(runner.State);
            return JsonUtility.ToJson(saved);
        }

        public void RestoreState(string json)
        {
            var saved = JsonUtility.FromJson<SavedQuests>(json);
            if (saved == null) return;
            DisposeRunners();
            foreach (var state in saved.States)
            {
                var quest = _autoStart.Find(q => q != null && q.Id == state.QuestId);
                if (quest == null) { Debug.LogWarning($"Saved quest '{state.QuestId}' is unknown; skipped.", this); continue; }
                Run(new QuestRunner(quest, state));
            }
        }

        private void StartAutoQuests()
        {
            foreach (var quest in _autoStart)
                if (quest != null) StartQuest(quest);
        }

        private void Run(QuestRunner runner)
        {
            runner.PhaseStarted += OnPhaseStarted;
            runner.Completed += OnCompleted;
            _runners[runner.Quest.Id] = runner;
            if (!runner.State.IsComplete) runner.Begin();
        }

        private void DisposeRunners()
        {
            foreach (var runner in _runners.Values) runner.Dispose();
            _runners.Clear();
        }

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
