using HollowChains.Core.Events;
using HollowChains.Gameplay.Player;
using UnityEngine;

namespace HollowChains.Gameplay.Interaction
{
    /// <summary>
    /// Finds the best <see cref="IInteractable"/> near the player (distance + facing), publishes its prompt to an event
    /// channel for the UI, and interacts on the Interact input. Setup: on the Player next to PlayerController;
    /// assign a CarryAnchor child transform and the prompt channel (EVT_InteractPrompt).
    /// </summary>
    public sealed class Interactor : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Transform _carryAnchor;
        [SerializeField] private StringEventChannelSO _onPromptChanged;
        [SerializeField, Min(0.1f)] private float _radius = 1.8f;
        [SerializeField, Range(0f, 1f)] private float _facingWeight = 0.6f;
        [SerializeField, Min(0.02f)] private float _scanInterval = 0.1f;

        private readonly Collider[] _hits = new Collider[16];
        private float _nextScan;
        private IInteractable _focused;

        public PlayerController Player => _player;

        public Transform CarryAnchor => _carryAnchor != null ? _carryAnchor : transform;

        public IInteractable Focused => _focused;

        private void Reset() => _player = GetComponent<PlayerController>();

        private void Start()
        {
            if (_player != null && _player.Input != null) _player.Input.InteractPressed += TryInteract;
        }

        private void OnDestroy()
        {
            if (_player != null && _player.Input != null) _player.Input.InteractPressed -= TryInteract;
        }

        private void Update()
        {
            if (Time.time < _nextScan) return;
            _nextScan = Time.time + _scanInterval;
            SetFocus(_player != null && _player.IsLocked ? null : FindBest());
        }

        public void TryInteract()
        {
            if (_focused == null || (_player != null && _player.IsLocked)) return;
            var target = _focused;
            target.Interact(this);
            _nextScan = 0f; // re-evaluate right away (the bucket is gone, prompt should hide)
        }

        private IInteractable FindBest()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, _radius, _hits, ~0, QueryTriggerInteraction.Collide);
            IInteractable best = null;
            float bestScore = float.MinValue;
            var forward = transform.forward;
            for (int i = 0; i < count; i++)
            {
                var candidate = _hits[i].GetComponentInParent<IInteractable>();
                if (candidate == null || !candidate.CanInteract(this)) continue;

                var toTarget = candidate.Position - transform.position;
                toTarget.y = 0f;
                float distance01 = 1f - Mathf.Clamp01(toTarget.magnitude / _radius);
                float facing01 = toTarget.sqrMagnitude > 0.0001f ? (Vector3.Dot(forward, toTarget.normalized) + 1f) * 0.5f : 1f;
                float score = Mathf.Lerp(distance01, facing01, _facingWeight);
                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }
            return best;
        }

        private void SetFocus(IInteractable next)
        {
            if (ReferenceEquals(next, _focused)) return;
            _focused = next;
            if (_onPromptChanged != null) _onPromptChanged.Raise(next != null ? next.Prompt : string.Empty);
        }
    }
}
