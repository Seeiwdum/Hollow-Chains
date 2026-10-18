using HollowChains.Core.Services;
using HollowChains.Data.Dialogue;
using UnityEngine;

namespace HollowChains.Narrative.Dialogue
{
    /// <summary>
    /// Villager whispers (SC-02 "village of hatred"): when the player passes within the trigger radius, plays one of
    /// the lines as an ambient subtitle (never interrupts story lines, never repeats the same line twice in a row).
    /// Setup: on a villager with a trigger SphereCollider; player is recognised by the "Player" tag.
    /// Spatial whisper audio is added in P5.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public sealed class BarkEmitter : MonoBehaviour
    {
        [SerializeField] private DialogueLineSO[] _lines;
        [SerializeField, Min(0f)] private float _cooldownSeconds = 12f;

        private float _nextAllowed;
        private int _lastIndex = -1;

        private void Reset()
        {
            var sphere = GetComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = 4f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _lines == null || _lines.Length == 0) return;
            if (Time.time < _nextAllowed) return;
            if (!ServiceLocator.TryGet<IDialogueService>(out var dialogue)) return;

            int index = _lines.Length == 1 ? 0 : (_lastIndex + 1 + Random.Range(0, _lines.Length - 1)) % _lines.Length;
            if (dialogue.TryPlayAmbient(_lines[index]))
            {
                _lastIndex = index;
                _nextAllowed = Time.time + _cooldownSeconds;
            }
        }
    }
}
