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

        protected override void OnInteract(Interactor who)
        {
            if (_line != null && _playLine != null) _playLine.Raise(_line);
        }
    }
}
