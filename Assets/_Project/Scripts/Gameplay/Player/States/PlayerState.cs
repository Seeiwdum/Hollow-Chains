using HollowChains.Core.StateMachines;

namespace HollowChains.Gameplay.Player.States
{
    /// <summary>
    /// Base for player movement states. Holds the owning controller; subclasses only decide how to move
    /// and when to hand over to another state.
    /// </summary>
    public abstract class PlayerState : IState
    {
        protected PlayerState(PlayerController owner) => Owner = owner;

        protected PlayerController Owner { get; }

        public virtual void Enter() { }

        public abstract void Tick(float deltaTime);

        public virtual void Exit() { }
    }
}
