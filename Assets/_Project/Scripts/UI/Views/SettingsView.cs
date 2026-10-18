using System;
using System.Collections.Generic;
using HollowChains.Core.Events;
using HollowChains.Core.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Demo settings (GDD §6 + accessibility B-603): master / music / SFX volume, subtitle size, invert camera Y,
    /// reduce flashes, press instead of hold. Opens on EVT_UI_OpenSettings (from Title or Pause), closes on
    /// EVT_UI_CloseSettings; every change is saved immediately through <see cref="GameSettings"/>.
    /// Setup: on the Settings panel with CanvasGroup.
    /// </summary>
    public sealed class SettingsView : UIView
    {
        [SerializeField] private VoidEventChannelSO _openRequested;
        [SerializeField] private VoidEventChannelSO _closeRequested;
        [SerializeField] private Slider _master;
        [SerializeField] private Slider _music;
        [SerializeField] private Slider _sfx;
        [SerializeField] private Slider _subtitleScale;
        [SerializeField] private Toggle _invertY;
        [SerializeField] private Toggle _reduceFlashes;
        [SerializeField] private Toggle _pressInsteadOfHold;
        [SerializeField] private Selectable _firstSelected;

        private readonly List<Action> _unbind = new List<Action>();
        private bool _loading;

        private void OnEnable()
        {
            if (_openRequested != null) _openRequested.Subscribe(Show);
            if (_closeRequested != null) _closeRequested.Subscribe(Hide);
            Bind(_master, v => GameSettings.Current.MasterVolume = v);
            Bind(_music, v => GameSettings.Current.MusicVolume = v);
            Bind(_sfx, v => GameSettings.Current.SfxVolume = v);
            Bind(_subtitleScale, v => GameSettings.Current.SubtitleScale = v);
            Bind(_invertY, on => GameSettings.Current.InvertCameraY = on);
            Bind(_reduceFlashes, on => GameSettings.Current.ReduceFlashes = on);
            Bind(_pressInsteadOfHold, on => GameSettings.Current.PressInsteadOfHold = on);
        }

        private void OnDisable()
        {
            if (_openRequested != null) _openRequested.Unsubscribe(Show);
            if (_closeRequested != null) _closeRequested.Unsubscribe(Hide);
            foreach (var unbind in _unbind) unbind();
            _unbind.Clear();
        }

        protected override void OnShow()
        {
            _loading = true;
            var s = GameSettings.Current;
            if (_master != null) _master.value = s.MasterVolume;
            if (_music != null) _music.value = s.MusicVolume;
            if (_sfx != null) _sfx.value = s.SfxVolume;
            if (_subtitleScale != null) _subtitleScale.value = s.SubtitleScale;
            if (_invertY != null) _invertY.isOn = s.InvertCameraY;
            if (_reduceFlashes != null) _reduceFlashes.isOn = s.ReduceFlashes;
            if (_pressInsteadOfHold != null) _pressInsteadOfHold.isOn = s.PressInsteadOfHold;
            _loading = false;
            if (_firstSelected != null && UnityEngine.EventSystems.EventSystem.current != null)
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
        }

        private void Bind(Slider slider, Action<float> apply)
        {
            if (slider == null) return;
            UnityEngine.Events.UnityAction<float> handler = v => Apply(() => apply(v));
            slider.onValueChanged.AddListener(handler);
            _unbind.Add(() => slider.onValueChanged.RemoveListener(handler));
        }

        private void Bind(Toggle toggle, Action<bool> apply)
        {
            if (toggle == null) return;
            UnityEngine.Events.UnityAction<bool> handler = on => Apply(() => apply(on));
            toggle.onValueChanged.AddListener(handler);
            _unbind.Add(() => toggle.onValueChanged.RemoveListener(handler));
        }

        private void Apply(Action change)
        {
            if (_loading) return;
            change();
            GameSettings.Save();
        }
    }
}
