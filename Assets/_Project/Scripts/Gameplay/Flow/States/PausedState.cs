namespace HollowChains.Gameplay.Flow.States
{
    /// <summary>Pause menu: time stopped, input off, cursor free.</summary>
    public sealed class PausedState : FlowState
    {
        public PausedState(GameFlowService owner) : base(owner) { }
        public override string Name => GameFlowService.StatePaused;
        protected override bool GameplayInput => false;
        protected override bool Paused => true;
    }
}
