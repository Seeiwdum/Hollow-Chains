using HollowChains.Core.Events;
using HollowChains.Core.Settings;
using HollowChains.Data.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace HollowChains.Gameplay.Interaction
{
    /// <summary>
    /// Shared behaviour of interactables (template method): <see cref="Interact"/> checks
    /// <see cref="CanInteract"/>, runs the subclass's <see cref="OnInteract"/>, then raises the optional event channel
    /// (e.g. quest "bucket picked up"). Needs a Collider on this GameObject or a child. Greybox colour: yellow.
    /// </summary>
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] private LocalizedString _localizedPrompt = new LocalizedString();
        [Tooltip("Dev fallback when no table entry is set.")]
        [SerializeField] private string _prompt = "Interact";
        [SerializeField] private VoidEventChannelSO _onInteracted;
        [Tooltip("Optional one-shot sound at the object (door latch, bucket handle…).")]
        [SerializeField] private AudioClip _sfx;
        [SerializeField, Range(0f, 1f)] private float _sfxVolume = 0.8f;

        public string Prompt => LocalizedText.Resolve(_localizedPrompt, _prompt);

        public Vector3 Position => transform.position;

        public virtual bool CanInteract(Interactor who) => isActiveAndEnabled;

        public void Interact(Interactor who)
        {
            if (!CanInteract(who)) return;
            OnInteract(who);
            if (_onInteracted != null) _onInteracted.Raise();
            if (_sfx != null)
            {
                var s = GameSettings.Current;
                AudioSource.PlayClipAtPoint(_sfx, transform.position, _sfxVolume * s.MasterVolume * s.SfxVolume);
            }
        }

        protected abstract void OnInteract(Interactor who);
    }
}
