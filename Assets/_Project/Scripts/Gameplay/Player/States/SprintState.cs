namespace HollowChains.Gameplay.Player.States
{
    /// <summary>Full sprint (SC-03 run to the cliff). Only reachable where sprint is allowed.</summary>
    public sealed class SprintState : PlayerState
    {
        public SprintState(PlayerController owner) : base(owner) { }

        public override void Tick(float deltaTime)
        {
            if (!Owner.WantsSprint) { Owner.ChangeTo(Owner.Locomotion); return; }

            Owner.Motor.Move(Owner.CameraRelative(Owner.MoveInput), Owner.Config.SprintSpeed, Owner.Config, deltaTime);
        }
    }
}
