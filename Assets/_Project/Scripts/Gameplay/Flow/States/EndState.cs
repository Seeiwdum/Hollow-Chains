namespace HollowChains.Gameplay.Flow.States
{
    /// <summary>Demo end screen ("To be continued").</summary>
    public sealed class EndState : FlowState
    {
        public EndState(GameFlowService owner) : base(owner) { }
        public override string Name => GameFlowService.StateEnd;
        protected override bool GameplayInput => false;
        protected override bool Paused => false;
    }
}
