using HollowChains.Core.Events;
using HollowChains.Core.Services;
using HollowChains.Core.StateMachines;
using HollowChains.Gameplay.Flow.States;
using HollowChains.Gameplay.Quests;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HollowChains.Gameplay.Flow
{
    /// <summary>
    /// Owns the game-flow state machine (Title → Playing ⇄ Paused → End). Menus talk to it only through event
    /// channels (UI never references gameplay, D-020) and listen to EVT_GameStateChanged to show/hide.
    /// Setup: on "Game" in _Boot; assign the UI request channels, the Pause action and EVT_DemoEnded.
    /// </summary>
    public sealed class GameFlowService : MonoBehaviour
    {
        public const string StateTitle = "Title";
        public const string StatePlaying = "Playing";
        public const string StatePaused = "Paused";
        public const string StateEnd = "End";

        [SerializeField] private string _firstScene = "S01_Bedroom";
        [SerializeField] private InputActionReference _pauseAction;

        [Header("Announces")]
        [SerializeField] private StringEventChannelSO _onStateChanged;
        [SerializeField] private BoolEventChannelSO _onGameplayInputEnabled;

        [Header("Listens (raised by menus / story)")]
        [SerializeField] private VoidEventChannelSO _newGameRequested;
        [SerializeField] private VoidEventChannelSO _resumeRequested;
        [SerializeField] private VoidEventChannelSO _quitToTitleRequested;
        [SerializeField] private VoidEventChannelSO _quitGameRequested;
        [SerializeField] private VoidEventChannelSO _demoEnded;

        private readonly StateMachine _machine = new StateMachine();
        private TitleState _title;
        private PlayingState _playing;
        private PausedState _paused;
        private EndState _end;

        public string CurrentState => _machine.Current is FlowState s ? s.Name : string.Empty;

        private void Awake()
        {
            _title = new TitleState(this);
            _playing = new PlayingState(this);
            _paused = new PausedState(this);
            _end = new EndState(this);
        }

        private void OnEnable()
        {
            Listen(_newGameRequested, StartNewGame, true);
            Listen(_resumeRequested, Resume, true);
            Listen(_quitToTitleRequested, QuitToTitle, true);
            Listen(_quitGameRequested, QuitGame, true);
            Listen(_demoEnded, ShowEnd, true);
            if (_pauseAction != null)
            {
                _pauseAction.action.Enable();
                _pauseAction.action.performed += OnPausePressed;
            }
        }

        private void OnDisable()
        {
            Listen(_newGameRequested, StartNewGame, false);
            Listen(_resumeRequested, Resume, false);
            Listen(_quitToTitleRequested, QuitToTitle, false);
            Listen(_quitGameRequested, QuitGame, false);
            Listen(_demoEnded, ShowEnd, false);
            if (_pauseAction != null) _pauseAction.action.performed -= OnPausePressed;
        }

        /// <summary>Called by the bootstrapper when the game starts from _Boot.</summary>
        public void EnterTitle() => _machine.ChangeState(_title);

        /// <summary>Called by the bootstrapper when Play was pressed inside a content scene.</summary>
        public void EnterPlaying() => _machine.ChangeState(_playing);

        public void StartNewGame()
        {
            if (ServiceLocator.TryGet<IQuestService>(out var quests)) quests.ResetAll();
            if (ServiceLocator.TryGet<ISceneFlowService>(out var flow)) flow.Load(_firstScene);
            _machine.ChangeState(_playing);
        }

        public void Pause()
        {
            if (_machine.Current == _playing) _machine.ChangeState(_paused);
        }

        public void Resume()
        {
            if (_machine.Current == _paused) _machine.ChangeState(_playing);
        }

        public void QuitToTitle()
        {
            if (ServiceLocator.TryGet<ISceneFlowService>(out var flow)) flow.UnloadCurrent();
            _machine.ChangeState(_title);
        }

        public void ShowEnd() => _machine.ChangeState(_end);

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>Shared per-state setup, called by <see cref="FlowState.Enter"/>.</summary>
        internal void ApplyFrame(bool paused, bool gameplayInput, string stateName)
        {
            Time.timeScale = paused ? 0f : 1f;
            Cursor.lockState = gameplayInput ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !gameplayInput;
            if (_onGameplayInputEnabled != null) _onGameplayInputEnabled.Raise(gameplayInput);
            if (_onStateChanged != null) _onStateChanged.Raise(stateName);
        }

        private void OnPausePressed(InputAction.CallbackContext _)
        {
            if (_machine.Current == _playing) Pause();
            else if (_machine.Current == _paused) Resume();
        }

        private static void Listen(VoidEventChannelSO channel, System.Action handler, bool subscribe)
        {
            if (channel == null) return;
            if (subscribe) channel.Subscribe(handler);
            else channel.Unsubscribe(handler);
        }
    }
}
