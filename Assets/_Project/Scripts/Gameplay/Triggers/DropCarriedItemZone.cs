using HollowChains.Core.Events;
using HollowChains.Gameplay.Interaction;
using HollowChains.Gameplay.Player;
using UnityEngine;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>
    /// Makes the player drop what they carry (D-015: the bucket falls when Phineas starts running at the scream).
    /// Raises EVT_ItemDropped so the persistent player state forgets the item.
    /// </summary>
    public sealed class DropCarriedItemZone : PlayerTriggerZone
    {
        [SerializeField] private VoidEventChannelSO _onItemDropped;
        [SerializeField] private float _forwardImpulse = 2.5f;

        protected override void OnPlayerEnter(PlayerController player)
        {
            var interactor = player.GetComponent<Interactor>();
            if (interactor == null || interactor.Carried == null) return;
            interactor.Drop(player.transform.forward * _forwardImpulse + Vector3.up * 1.5f);
            if (_onItemDropped != null) _onItemDropped.Raise();
        }
    }
}
