namespace HollowChains.Core.StateMachines
{
    /// <summary>One state of a <see cref="StateMachine"/> (player movement, quest phase, game flow…).</summary>
    public interface IState
    {
        void Enter();

        void Tick(float deltaTime);

        void Exit();
    }
}
