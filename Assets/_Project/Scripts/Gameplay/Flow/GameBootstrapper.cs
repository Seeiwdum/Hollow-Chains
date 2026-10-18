using HollowChains.Core.Services;
using HollowChains.Gameplay.Quests;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HollowChains.Gameplay.Flow
{
    /// <summary>
    /// Entry point in the _Boot scene. Registers global services, then either shows the title screen or — when Play
    /// was pressed inside a content scene in the editor — adopts that scene and goes straight to Playing.
    /// Setup: on "Game" in _Boot together with SceneFlowService, QuestService and GameFlowService.
    /// </summary>
    public sealed class GameBootstrapper : MonoBehaviour
    {
        public const string BootSceneName = "_Boot";

        [SerializeField] private SceneFlowService _sceneFlow;
        [SerializeField] private QuestService _quests;
        [SerializeField] private GameFlowService _gameFlow;

        private static GameBootstrapper s_active;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => s_active = null;

        private void Awake()
        {
            if (s_active != null)
            {
                // A second _Boot was loaded (Play pressed inside _Boot while the loader also added one): drop it.
                enabled = false;
                SceneManager.UnloadSceneAsync(gameObject.scene);
                return;
            }
            s_active = this;
            if (_sceneFlow == null) _sceneFlow = GetComponent<SceneFlowService>();
            if (_quests == null) _quests = GetComponent<QuestService>();
            if (_gameFlow == null) _gameFlow = GetComponent<GameFlowService>();
            ServiceLocator.Register<ISceneFlowService>(_sceneFlow);
            if (_quests != null) ServiceLocator.Register<IQuestService>(_quests);
        }

        private void Start()
        {
            if (s_active != this) return;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name != BootSceneName && scene.isLoaded)
                {
                    _sceneFlow.Adopt(scene);
                    _gameFlow.EnterPlaying();
                    return;
                }
            }
            _gameFlow.EnterTitle();
        }

        private void OnDestroy()
        {
            if (s_active != this) return;
            s_active = null;
            ServiceLocator.Unregister<ISceneFlowService>();
            ServiceLocator.Unregister<IQuestService>();
        }
    }
}
