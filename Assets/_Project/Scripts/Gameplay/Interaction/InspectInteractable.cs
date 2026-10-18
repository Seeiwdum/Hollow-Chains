using HollowChains.Core.Events;
using UnityEngine;

namespace HollowChains.Gameplay.Interaction
{
    /// <summary>
    /// Story prop (torn painting, Elena's flower, old chain): shows Phineas' thought line through an event channel
    /// listened to by the UI. Placeholder text until Dialogue + Localization arrive in P3.
    /// </summary>
    public sealed class InspectInteractable : InteractableBase
    {
        [SerializeField, TextArea] private string _thought = "...";
        [SerializeField] private StringEventChannelSO _onThought;

        protected override void OnInteract(Interactor who)
        {
            if (_onThought != null) _onThought.Raise(_thought);
        }
    }
}
