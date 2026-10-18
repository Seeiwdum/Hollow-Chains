using HollowChains.Core.Events;
using HollowChains.Core.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Minimal demo settings (GDD §6): master / music / SFX volume, subtitle size, invert camera Y.
    /// Opens on EVT_UI_OpenSettings (from Title or Pause), closes on EVT_UI_CloseSettings; every change is saved
    /// immediately through <see cref="GameSettings"/>. Setup: on the Settings panel with CanvasGroup.
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
        [SerializeField] private Selectable _firstSelected;

        private bool _loading;

        private void OnEnable()
        {
            if (_openRequested != null) _openRequested.Subscribe(Show);
            if (_closeRequested != null) _closeRequested.Subscribe(Hide);
            Bind(_master, v => GameSettings.Current.MasterVolume = v);
            Bind(_music, v => GameSettings.Current.MusicVolume = v);
            Bind(_sfx, v => GameSettings.Current.SfxVolume = v);
            Bind(_subtitleScale, v => GameSettings.Current.SubtitleScale = v);
            if (_invertY != null) _invertY.onValueChanged.AddListener(OnInvert);
        }

        private void OnDisable()
        {
            if (_openRequested != null) _openRequested.Unsubscribe(Show);
            if (_closeRequested != null) _closeRequested.Unsubscribe(Hide);
            foreach (var s in new[] { _master, _music, _sfx, _subtitleScale }) if (s != null) s.onValueChanged.RemoveAllListeners();
            if (_invertY != null) _invertY.onValueChanged.RemoveListener(OnInvert);
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
            _loading = false;
            if (_firstSelected != null && UnityEngine.EventSystems.EventSystem.current != null)
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
        }

        private void Bind(Slider slider, System.Action<float> apply)
        {
            if (slider == null) return;
            slider.onValueChanged.AddListener(v =>
            {
                if (_loading) return;
                apply(v);
                GameSettings.Save();
            });
        }

        private void OnInvert(bool on)
        {
            if (_loading) return;
            GameSettings.Current.InvertCameraY = on;
            GameSettings.Save();
        }
    }
}
