namespace HollowChains.Gameplay.Flow.States
{
    /// <summary>Normal play: input on, cursor locked.</summary>
    public sealed class PlayingState : FlowState
    {
        public PlayingState(GameFlowService owner) : base(owner) { }
        public override string Name => GameFlowService.StatePlaying;
        protected override bool GameplayInput => true;
        protected override bool Paused => false;
    }
}
