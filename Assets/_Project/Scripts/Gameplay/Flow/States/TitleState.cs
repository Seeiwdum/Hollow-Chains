namespace HollowChains.Gameplay.Flow.States
{
    /// <summary>Title screen over a dark backdrop; no content scene loaded.</summary>
    public sealed class TitleState : FlowState
    {
        public TitleState(GameFlowService owner) : base(owner) { }
        public override string Name => GameFlowService.StateTitle;
        protected override bool GameplayInput => false;
        protected override bool Paused => false;
    }
}
