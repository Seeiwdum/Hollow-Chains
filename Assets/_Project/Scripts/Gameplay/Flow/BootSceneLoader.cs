using UnityEngine;
using UnityEngine.SceneManagement;

namespace HollowChains.Gameplay.Flow
{
    /// <summary>
    /// Makes sure _Boot (camera, services, UI) is always present: if Play starts in a content scene,
    /// _Boot is loaded additively. No setup needed; _Boot must be in Build Settings.
    /// A second bootstrapper (Play pressed inside _Boot) is ignored by <see cref="GameBootstrapper"/>.
    /// </summary>
    internal static class BootSceneLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureBootScene()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).name == GameBootstrapper.BootSceneName) return;

            if (!Application.CanStreamedLevelBeLoaded(GameBootstrapper.BootSceneName))
            {
                Debug.LogWarning("_Boot is not in Build Settings; running scene without boot services.");
                return;
            }
            SceneManager.LoadScene(GameBootstrapper.BootSceneName, LoadSceneMode.Additive);
        }
    }
}
