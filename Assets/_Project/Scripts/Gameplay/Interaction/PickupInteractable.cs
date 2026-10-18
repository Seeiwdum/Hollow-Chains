using UnityEngine;

namespace HollowChains.Gameplay.Interaction
{
    /// <summary>
    /// Carryable key item (the bucket, ITEM_BUCKET): on interact it attaches to the player's carry anchor and puts
    /// the player in the Carry state. Colliders are disabled while carried.
    /// </summary>
    public sealed class PickupInteractable : InteractableBase
    {
        [SerializeField] private string _itemId = "ITEM_BUCKET";

        public string ItemId => _itemId;

        public override bool CanInteract(Interactor who) => base.CanInteract(who) && !who.Player.IsCarrying;

        protected override void OnInteract(Interactor who)
        {
            foreach (var c in GetComponentsInChildren<Collider>()) c.enabled = false;
            transform.SetParent(who.CarryAnchor, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            who.Player.SetCarrying(true);
        }
    }
}
