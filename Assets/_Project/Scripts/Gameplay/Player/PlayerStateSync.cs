using System.Collections.Generic;
using HollowChains.Core.Events;
using HollowChains.Core.Services;
using HollowChains.Data.Items;
using HollowChains.Gameplay.Interaction;
using HollowChains.Gameplay.Stats;
using UnityEngine;

namespace HollowChains.Gameplay.Player
{
    /// <summary>
    /// Bridges this scene's Player with the persistent <see cref="IPlayerStateService"/>: on Start restores HP and the
    /// carried item (spawning its carry prefab in hand), then reports HP changes, pickups and drops back.
    /// Setup: on the Player next to Health and Interactor; list the known items (ITEM_BUCKET).
    /// </summary>
    public sealed class PlayerStateSync : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Interactor _interactor;
        [SerializeField] private List<ItemSO> _knownItems = new List<ItemSO>();
        [SerializeField] private StringEventChannelSO _onItemPicked;
        [SerializeField] private VoidEventChannelSO _onItemDropped;

        private IPlayerStateService _service;

        private void Reset()
        {
            _health = GetComponent<Health>();
            _interactor = GetComponent<Interactor>();
        }

        private void OnEnable()
        {
            if (_onItemPicked != null) _onItemPicked.Subscribe(OnPicked);
            if (_onItemDropped != null) _onItemDropped.Subscribe(OnDropped);
        }

        private void OnDisable()
        {
            if (_onItemPicked != null) _onItemPicked.Unsubscribe(OnPicked);
            if (_onItemDropped != null) _onItemDropped.Unsubscribe(OnDropped);
            if (_health != null) _health.Changed -= OnHealthChanged;
        }

        private void Start()
        {
            if (!ServiceLocator.TryGet(out _service)) return;
            var state = _service.State;
            if (_health != null)
            {
                if (state.Health >= 0) _health.SetTo(state.Health);
                _health.Changed += OnHealthChanged;
            }
            RestoreCarried(state.CarriedItemId);
        }

        private void RestoreCarried(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _interactor == null || _interactor.Carried != null) return;
            var item = _knownItems.Find(i => i != null && i.Id == itemId);
            if (item == null || item.CarryPrefab == null)
            {
                Debug.LogWarning($"{name}: cannot restore carried item '{itemId}' (not in Known Items or no carry prefab).", this);
                return;
            }
            _interactor.Hold(Instantiate(item.CarryPrefab));
        }

        private void OnHealthChanged(int current, int max) => _service?.RecordHealth(current);

        private void OnPicked(string itemId) => _service?.RecordCarried(itemId);

        private void OnDropped() => _service?.RecordCarried(string.Empty);
    }
}
