using System;
using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Hides one UI element (alpha 0, no raycasts) while the active content scene or the game state is on a block list.
    /// It only adds a veto on top of whatever else controls visibility: a parent <see cref="GameStateView"/> still
    /// decides when the element may show, this gate decides when it must not.
    /// Setup: on the element's GameObject (a CanvasGroup is added automatically). Examples:
    /// HUD/HealthBar in _Boot: Hidden In Scenes = S00_ComicIntro_Placeholder, no channel needed.
    /// SkipHint in S00: Hidden In States = Paused, On State Changed = EVT_GameStateChanged.
    /// Reads UnityEngine.SceneManagement and Core event channels only; no Gameplay or Narrative reference.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class CanvasGroupGate : MonoBehaviour
    {
        [Tooltip("Optional. Needed only when Hidden In States is used (EVT_GameStateChanged).")]
        [SerializeField] private StringEventChannelSO _onStateChanged;
        [SerializeField] private string[] _hiddenInStates = Array.Empty<string>();
        [SerializeField] private string[] _hiddenInScenes = { "S00_ComicIntro_Placeholder" };

        private CanvasGroup _group;
        private bool _sceneHidden;
        private bool _stateHidden;

        /// <summary>True when <paramref name="value"/> is in <paramref name="list"/> (ordinal match; null-safe).</summary>
        public static bool IsListed(string[] list, string value)
        {
            if (list == null || string.IsNullOrEmpty(value)) return false;
            return Array.IndexOf(list, value) >= 0;
        }

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            if (_onStateChanged != null) _onStateChanged.Subscribe(OnState);
            _sceneHidden = IsListed(_hiddenInScenes, SceneManager.GetActiveScene().name);
            Apply();
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            if (_onStateChanged != null) _onStateChanged.Unsubscribe(OnState);
        }

        private void OnActiveSceneChanged(Scene previous, Scene next)
        {
            _sceneHidden = IsListed(_hiddenInScenes, next.name);
            Apply();
        }

        private void OnState(string state)
        {
            _stateHidden = IsListed(_hiddenInStates, state);
            Apply();
        }

        private void Apply()
        {
            bool visible = !_sceneHidden && !_stateHidden;
            _group.alpha = visible ? 1f : 0f;
            _group.blocksRaycasts = visible;
        }
    }
}
