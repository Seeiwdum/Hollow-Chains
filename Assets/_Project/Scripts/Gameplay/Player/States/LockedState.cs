namespace HollowChains.Gameplay.Player.States
{
    /// <summary>No player movement (cutscenes, dialogue). Left only through PlayerController.ExitCutscene.</summary>
    public sealed class LockedState : PlayerState
    {
        public LockedState(PlayerController owner) : base(owner) { }

        public override void Tick(float deltaTime) => Owner.Motor.Halt(Owner.Config, deltaTime);
    }
}
