using System;
using System.Collections;
using HollowChains.Core.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HollowChains.Gameplay.Flow
{
    /// <summary>
    /// <see cref="ISceneFlowService"/> implementation living in the _Boot scene.
    /// Fades out (optional CanvasGroup), unloads the current content scene, loads the next one additively,
    /// makes it active, then fades back in. Registered by <see cref="GameBootstrapper"/>.
    /// </summary>
    public sealed class SceneFlowService : MonoBehaviour, ISceneFlowService
    {
        [Tooltip("Full-screen black CanvasGroup used for fades. Optional.")]
        [SerializeField] private CanvasGroup _fade;
        [SerializeField, Min(0f)] private float _fadeSeconds = 0.35f;

        public bool IsLoading { get; private set; }

        public string CurrentScene { get; private set; }

        public event Action<string> SceneLoaded;

        public void Load(string sceneName)
        {
            if (IsLoading || string.IsNullOrEmpty(sceneName)) return;
            StartCoroutine(LoadRoutine(sceneName));
        }

        public void UnloadCurrent()
        {
            if (IsLoading || string.IsNullOrEmpty(CurrentScene)) return;
            StartCoroutine(UnloadRoutine());
        }

        /// <summary>Registers a content scene that was already open (e.g. pressing Play inside S02 in the editor).</summary>
        public void Adopt(Scene scene)
        {
            CurrentScene = scene.name;
            SceneManager.SetActiveScene(scene);
            SceneLoaded?.Invoke(CurrentScene);
        }

        private IEnumerator LoadRoutine(string sceneName)
        {
            IsLoading = true;
            yield return Fade(0f, 1f);

            if (!string.IsNullOrEmpty(CurrentScene))
            {
                var current = SceneManager.GetSceneByName(CurrentScene);
                if (current.isLoaded) yield return SceneManager.UnloadSceneAsync(current);
            }

            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var loaded = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(loaded);
            CurrentScene = sceneName;

            yield return Fade(1f, 0f);
            IsLoading = false;
            SceneLoaded?.Invoke(sceneName);
        }

        private IEnumerator UnloadRoutine()
        {
            IsLoading = true;
            yield return Fade(0f, 1f);
            var current = SceneManager.GetSceneByName(CurrentScene);
            if (current.isLoaded) yield return SceneManager.UnloadSceneAsync(current);
            CurrentScene = null;
            yield return Fade(1f, 0f);
            IsLoading = false;
        }

        private IEnumerator Fade(float from, float to)
        {
            if (_fade == null || _fadeSeconds <= 0f) yield break;
            _fade.blocksRaycasts = true;
            for (float t = 0f; t < _fadeSeconds; t += Time.unscaledDeltaTime)
            {
                _fade.alpha = Mathf.Lerp(from, to, t / _fadeSeconds);
                yield return null;
            }
            _fade.alpha = to;
            _fade.blocksRaycasts = to > 0.5f;
        }
    }
}
