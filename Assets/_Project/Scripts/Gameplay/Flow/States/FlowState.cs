using HollowChains.Core.StateMachines;

namespace HollowChains.Gameplay.Flow.States
{
    /// <summary>
    /// Base for game-flow states (Title, Playing, Paused, End). Each state decides time scale, cursor and whether
    /// gameplay input is on, and announces itself so menus can show/hide (template method: <see cref="OnEnter"/>).
    /// </summary>
    public abstract class FlowState : IState
    {
        protected FlowState(GameFlowService owner) => Owner = owner;

        protected GameFlowService Owner { get; }

        /// <summary>Name published on EVT_GameStateChanged; menus match against it.</summary>
        public abstract string Name { get; }

        protected abstract bool GameplayInput { get; }

        protected abstract bool Paused { get; }

        public void Enter()
        {
            Owner.ApplyFrame(Paused, GameplayInput, Name);
            OnEnter();
        }

        public virtual void Tick(float deltaTime) { }

        public virtual void Exit() { }

        protected virtual void OnEnter() { }
    }
}
