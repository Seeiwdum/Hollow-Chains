using HollowChains.Core.Events;
using HollowChains.Data.Dialogue;
using UnityEngine;

namespace HollowChains.Gameplay.Interaction
{
    /// <summary>
    /// Story prop (torn painting, Elena's flower, old chain, the pocket watch): asks the dialogue system to play
    /// Phineas' thought line through the EVT_PlayLine channel (gameplay never references Narrative directly).
    /// </summary>
    public sealed class InspectInteractable : InteractableBase
    {
        [SerializeField] private DialogueLineSO _line;
        [SerializeField] private DialogueLineEventChannelSO _playLine;

        [Header("Legacy (P2 placeholder) — migrated to _line in P3.2")]
        [SerializeField, TextArea] private string _thought = "...";
        [SerializeField] private StringEventChannelSO _onThought;

        protected override void OnInteract(Interactor who)
        {
            if (_line != null && _playLine != null) _playLine.Raise(_line);
            else if (_onThought != null) _onThought.Raise(_thought);
        }
    }
}
