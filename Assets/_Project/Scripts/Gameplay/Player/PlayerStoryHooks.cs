using HollowChains.Core.Events;
using HollowChains.Core.Services;
using HollowChains.Gameplay.Stats;
using UnityEngine;

namespace HollowChains.Gameplay.Player
{
    /// <summary>
    /// How the gameplay Player reacts to story events raised by cutscenes (timelines never call gameplay code):
    /// cutscene start → lock + hide (a cutscene actor takes over), wolf bite → HP = 1 (SC-04, CANON),
    /// Soul Drain → HP restored and Humanity drops (SC-05, D-002 data only).
    /// Setup: on the Player prefab; assign EVT_CutsceneStarted, EVT_PlayerBitten, EVT_SoulDrain.
    /// </summary>
    public sealed class PlayerStoryHooks : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Health _health;
        [SerializeField] private VoidEventChannelSO _cutsceneStarted;
        [SerializeField] private VoidEventChannelSO _bitten;
        [SerializeField] private VoidEventChannelSO _soulDrain;
        [SerializeField] private int _humanityCostOfSoulDrain = 15;

        private void Reset()
        {
            _player = GetComponent<PlayerController>();
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            if (_cutsceneStarted != null) _cutsceneStarted.Subscribe(OnCutsceneStarted);
            if (_bitten != null) _bitten.Subscribe(OnBitten);
            if (_soulDrain != null) _soulDrain.Subscribe(OnSoulDrain);
        }

        private void OnDisable()
        {
            if (_cutsceneStarted != null) _cutsceneStarted.Unsubscribe(OnCutsceneStarted);
            if (_bitten != null) _bitten.Unsubscribe(OnBitten);
            if (_soulDrain != null) _soulDrain.Unsubscribe(OnSoulDrain);
        }

        private void OnCutsceneStarted()
        {
            if (_player != null) _player.EnterCutscene();
            foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        }

        private void OnBitten()
        {
            if (_health != null) _health.SetTo(1);
        }

        private void OnSoulDrain()
        {
            if (_health != null) _health.Heal(_health.Max);
            if (ServiceLocator.TryGet<IPlayerStateService>(out var state)) state.ChangeHumanity(-_humanityCostOfSoulDrain);
        }
    }
}
