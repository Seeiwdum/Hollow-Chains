namespace HollowChains.Gameplay.Player.States
{
    /// <summary>Carrying the bucket (SC-01 → SC-03): slower, no sprint until the item is dropped.</summary>
    public sealed class CarryState : PlayerState
    {
        public CarryState(PlayerController owner) : base(owner) { }

        public override void Tick(float deltaTime)
        {
            if (!Owner.IsCarrying) { Owner.ChangeTo(Owner.Locomotion); return; }

            var input = Owner.MoveInput;
            var config = Owner.Config;
            float baseSpeed = input.magnitude >= config.RunThreshold ? config.RunSpeed : config.WalkSpeed;
            Owner.Motor.Move(Owner.CameraRelative(input), baseSpeed * config.CarryMultiplier, config, deltaTime);
        }
    }
}
