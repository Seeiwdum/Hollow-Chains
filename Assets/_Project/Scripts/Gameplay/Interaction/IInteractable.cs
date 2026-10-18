using UnityEngine;

namespace HollowChains.Gameplay.Interaction
{
    /// <summary>Anything the player can press Interact on. Found by <see cref="Interactor"/> through its colliders.</summary>
    public interface IInteractable
    {
        /// <summary>Short action text for the prompt, from the "UI" string table.</summary>
        string Prompt { get; }

        /// <summary>World position used to rank candidates (distance and facing).</summary>
        Vector3 Position { get; }

        bool CanInteract(Interactor who);

        void Interact(Interactor who);
    }
}
