using HollowChains.Gameplay.Player;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>Allows sprinting while the player is inside (SC-03 forest run).</summary>
    public sealed class SprintZone : PlayerTriggerZone
    {
        protected override void OnPlayerEnter(PlayerController player) => player.SetSprintAllowed(true);

        protected override void OnPlayerExit(PlayerController player) => player.SetSprintAllowed(false);
    }
}
