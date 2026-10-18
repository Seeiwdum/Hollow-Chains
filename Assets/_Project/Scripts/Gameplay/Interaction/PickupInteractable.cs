using HollowChains.Core.Events;
using UnityEngine;

namespace HollowChains.Gameplay.Interaction
{
    /// <summary>
    /// Carryable key item (the bucket, ITEM_BUCKET): on interact the Interactor holds it and the item ID is announced
    /// (EVT_ItemPicked) so persistent player state and quests can react.
    /// </summary>
    public sealed class PickupInteractable : InteractableBase
    {
        [SerializeField] private string _itemId = "ITEM_BUCKET";
        [SerializeField] private StringEventChannelSO _onPickedUp;

        public string ItemId => _itemId;

        public override bool CanInteract(Interactor who) => base.CanInteract(who) && !who.Player.IsCarrying;

        protected override void OnInteract(Interactor who)
        {
            who.Hold(gameObject);
            if (_onPickedUp != null) _onPickedUp.Raise(_itemId);
        }
    }
}
