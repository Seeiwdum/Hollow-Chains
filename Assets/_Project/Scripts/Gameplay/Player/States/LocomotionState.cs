namespace HollowChains.Gameplay.Player.States
{
    /// <summary>Walk / run depending on stick strength. Hands over to Sprint or Carry when their conditions hold.</summary>
    public sealed class LocomotionState : PlayerState
    {
        public LocomotionState(PlayerController owner) : base(owner) { }

        public override void Tick(float deltaTime)
        {
            if (Owner.IsCarrying) { Owner.ChangeTo(Owner.Carry); return; }
            if (Owner.WantsSprint) { Owner.ChangeTo(Owner.Sprint); return; }

            var input = Owner.MoveInput;
            var config = Owner.Config;
            float speed = input.magnitude >= config.RunThreshold ? config.RunSpeed : config.WalkSpeed;
            Owner.Motor.Move(Owner.CameraRelative(input), speed, config, deltaTime);
        }
    }
}
