using System;
using HollowChains.Data.Quests;

namespace HollowChains.Gameplay.Quests
{
    /// <summary>
    /// Drives one quest through its phases: watches the current phase's condition and advances when it is met.
    /// Plain C# (no MonoBehaviour) so it is unit-testable; owned by <see cref="QuestService"/>.
    /// </summary>
    public sealed class QuestRunner : IDisposable
    {
        private readonly QuestSO _quest;
        private IConditionWatcher _watcher;

        public QuestRunner(QuestSO quest, QuestState state = null)
        {
            _quest = quest ?? throw new ArgumentNullException(nameof(quest));
            State = state ?? new QuestState(quest.Id);
        }

        public QuestState State { get; }

        public QuestSO Quest => _quest;

        /// <summary>The phase the player is on, or null when the quest is complete.</summary>
        public QuestPhaseSO CurrentPhase =>
            State.IsComplete || State.PhaseIndex >= _quest.Phases.Count ? null : _quest.Phases[State.PhaseIndex];

        /// <summary>Raised when a new phase becomes current (also for the first phase on Begin).</summary>
        public event Action<QuestPhaseSO> PhaseStarted;

        public event Action<QuestSO> Completed;

        public void Begin()
        {
            if (_quest.Phases.Count == 0) { Complete(); return; }
            EnterCurrentPhase();
        }

        /// <summary>Completes the current phase immediately (debug, scripted beats).</summary>
        public void ForceAdvance() => OnPhaseConditionMet();

        public void Dispose()
        {
            _watcher?.Dispose();
            _watcher = null;
        }

        private void EnterCurrentPhase()
        {
            var phase = CurrentPhase;
            if (phase == null) { Complete(); return; }

            PhaseStarted?.Invoke(phase);
            _watcher?.Dispose();
            _watcher = phase.CompleteWhen != null ? phase.CompleteWhen.CreateWatcher() : null;
            _watcher?.Start(OnPhaseConditionMet); // a phase without a condition waits for ForceAdvance
        }

        private void OnPhaseConditionMet()
        {
            if (State.IsComplete) return;
            _watcher?.Dispose();
            _watcher = null;
            State.PhaseIndex++;
            if (State.PhaseIndex >= _quest.Phases.Count) Complete();
            else EnterCurrentPhase();
        }

        private void Complete()
        {
            if (State.IsComplete) return;
            State.IsComplete = true;
            Dispose();
            Completed?.Invoke(_quest);
        }
    }
}
