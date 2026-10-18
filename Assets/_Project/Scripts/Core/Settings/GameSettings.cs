using System;
using UnityEngine;

namespace HollowChains.Core.Settings
{
    /// <summary>
    /// Player options (demo minimum, GDD §6 + accessibility B-603): volumes, subtitle size, camera invert,
    /// softer screen flashes, press instead of hold for hold beats. Persisted in PlayerPrefs.
    /// Read anywhere through <see cref="Current"/>; the Settings menu calls <see cref="Save"/>, which raises
    /// <see cref="Changed"/> so listeners (audio, subtitles, camera) update immediately.
    /// </summary>
    [Serializable]
    public sealed class GameSettings
    {
        private const string Key = "HollowChains.Settings.v1";

        public float MasterVolume = 1f;
        public float MusicVolume = 0.8f;
        public float SfxVolume = 1f;
        public float SubtitleScale = 1f;
        public bool InvertCameraY;
        public bool ReduceFlashes;
        public bool PressInsteadOfHold;

        private static GameSettings s_current;

        public static GameSettings Current => s_current ??= Load();

        public static event Action<GameSettings> Changed;

        public static void Save()
        {
            var s = Current;
            s.MasterVolume = Mathf.Clamp01(s.MasterVolume);
            s.MusicVolume = Mathf.Clamp01(s.MusicVolume);
            s.SfxVolume = Mathf.Clamp01(s.SfxVolume);
            s.SubtitleScale = Mathf.Clamp(s.SubtitleScale, 0.75f, 1.5f);
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(s));
            PlayerPrefs.Save();
            Changed?.Invoke(s);
        }

        private static GameSettings Load()
        {
            var json = PlayerPrefs.GetString(Key, string.Empty);
            if (string.IsNullOrEmpty(json)) return new GameSettings();
            try { return JsonUtility.FromJson<GameSettings>(json) ?? new GameSettings(); }
            catch (ArgumentException) { return new GameSettings(); }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            s_current = null;
            Changed = null;
        }
    }
}
