using HollowChains.Core.Save;
using HollowChains.Core.Services;
using HollowChains.Gameplay.Stats;
using UnityEngine;

namespace HollowChains.Gameplay.Player
{
    /// <summary>
    /// <see cref="IPlayerStateService"/> + <see cref="ISaveable"/> ("player") in _Boot. Humanity lives here as data
    /// only (D-002). Setup: on "Game" in _Boot; registers itself (Awake) and its save entry (Start).
    /// </summary>
    public sealed class PlayerStateService : MonoBehaviour, IPlayerStateService, ISaveable
    {
        private PlayerStateData _state = new PlayerStateData();
        private HumanityModel _humanity = new HumanityModel();
        private bool _registered;

        public PlayerStateData State => _state;

        public int Humanity => _humanity.Value;

        public string SaveKey => "player";

        private void Awake()
        {
            if (ServiceLocator.TryGet<IPlayerStateService>(out _)) { enabled = false; return; } // duplicate _Boot
            ServiceLocator.Register<IPlayerStateService>(this);
            _registered = true;
        }

        private void Start()
        {
            if (_registered && ServiceLocator.TryGet<ISaveService>(out var save)) save.Register(this);
        }

        private void OnDestroy()
        {
            if (!_registered) return;
            if (ServiceLocator.TryGet<ISaveService>(out var save)) save.Unregister(this);
            ServiceLocator.Unregister<IPlayerStateService>();
        }

        public void RecordHealth(int current) => _state.Health = current;

        public void RecordCarried(string itemId) => _state.CarriedItemId = itemId ?? string.Empty;

        public void ChangeHumanity(int delta)
        {
            _humanity.Change(delta);
            _state.Humanity = _humanity.Value;
        }

        public void ResetToNew()
        {
            _state = new PlayerStateData();
            _humanity = new HumanityModel(_state.Humanity);
        }

        public string CaptureState() => JsonUtility.ToJson(_state);

        public void RestoreState(string json)
        {
            _state = JsonUtility.FromJson<PlayerStateData>(json) ?? new PlayerStateData();
            _humanity = new HumanityModel(_state.Humanity);
        }
    }
}
