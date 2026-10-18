using System;

namespace HollowChains.Core.StateMachines
{
    /// <summary>
    /// Minimal finite state machine. Owners call <see cref="Tick"/> from their Update and
    /// <see cref="ChangeState"/> when a transition happens; the machine guarantees Exit → Enter ordering.
    /// </summary>
    public sealed class StateMachine
    {
        private IState _current;

        public IState Current => _current;

        /// <summary>Raised after a transition with (previous, next). Previous is null on the first state.</summary>
        public event Action<IState, IState> StateChanged;

        public void ChangeState(IState next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            if (ReferenceEquals(next, _current)) return;

            var previous = _current;
            previous?.Exit();
            _current = next;
            _current.Enter();
            StateChanged?.Invoke(previous, _current);
        }

        public void Tick(float deltaTime) => _current?.Tick(deltaTime);
    }
}
