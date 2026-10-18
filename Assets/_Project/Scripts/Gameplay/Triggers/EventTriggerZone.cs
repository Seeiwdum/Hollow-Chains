using HollowChains.Core.Events;
using HollowChains.Gameplay.Player;
using UnityEngine;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>Raises an event channel when the player enters (quest phase reached, scream heard, checkpoint…).</summary>
    public sealed class EventTriggerZone : PlayerTriggerZone
    {
        [SerializeField] private VoidEventChannelSO _onEnter;

        protected override void OnPlayerEnter(PlayerController player)
        {
            if (_onEnter != null) _onEnter.Raise();
        }
    }
}
