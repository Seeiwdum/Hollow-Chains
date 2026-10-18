using HollowChains.Gameplay.Player;
using UnityEngine;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>
    /// Base for trigger volumes that react to the player (template method: subclasses override
    /// <see cref="OnPlayerEnter"/> / <see cref="OnPlayerExit"/>). Needs a Collider with "Is Trigger" on the same
    /// GameObject; the player is found through its PlayerController. Greybox colour: transparent blue (D-022).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class PlayerTriggerZone : MonoBehaviour
    {
        [Tooltip("Fire only the first time the player enters.")]
        [SerializeField] private bool _once;

        private bool _fired;

        private void Reset() => GetComponent<Collider>().isTrigger = true;

        private void OnTriggerEnter(Collider other)
        {
            if (_once && _fired) return;
            var player = other.GetComponentInParent<PlayerController>();
            if (player == null) return;
            _fired = true;
            OnPlayerEnter(player);
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (player != null) OnPlayerExit(player);
        }

        protected abstract void OnPlayerEnter(PlayerController player);

        protected virtual void OnPlayerExit(PlayerController player) { }
    }
}
