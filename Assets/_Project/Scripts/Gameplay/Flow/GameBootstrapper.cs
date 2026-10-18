using HollowChains.Core.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HollowChains.Gameplay.Flow
{
    /// <summary>
    /// Entry point in the _Boot scene. Registers global services, locks the cursor, then either loads the first
    /// content scene or adopts the one already open (when Play is pressed inside a content scene in the editor).
    /// Setup: on a GameObject in _Boot together with <see cref="SceneFlowService"/>.
    /// </summary>
    public sealed class GameBootstrapper : MonoBehaviour
    {
        public const string BootSceneName = "_Boot";

        [SerializeField] private SceneFlowService _sceneFlow;
        [SerializeField] private string _firstScene = "S01_Bedroom";
        [SerializeField] private bool _lockCursor = true;

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
            ServiceLocator.Register<ISceneFlowService>(_sceneFlow);
        }

        private void Start()
        {
            if (s_active != this) return;
            Begin();
        }

        private void Begin()
        {
            if (_lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name != BootSceneName && scene.isLoaded)
                {
                    _sceneFlow.Adopt(scene);
                    return;
                }
            }
            _sceneFlow.Load(_firstScene);
        }

        private void OnDestroy()
        {
            if (s_active != this) return;
            s_active = null;
            ServiceLocator.Unregister<ISceneFlowService>();
        }
    }
}
