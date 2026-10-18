using HollowChains.Core.Events;
using HollowChains.Core.Services;
using HollowChains.Data.Dialogue;
using UnityEngine;

namespace HollowChains.Narrative.Dialogue
{
    /// <summary>
    /// <see cref="IDialogueService"/> in the _Boot scene. Plays lines through a <see cref="SubtitleQueue"/>, listens to
    /// the "play line" channel raised by gameplay, and publishes formatted subtitle text for the UI
    /// (empty string = clear). Registers itself in the ServiceLocator.
    /// Setup: on the "Game" object in _Boot; assign EVT_PlayLine and EVT_Subtitle.
    /// </summary>
    public sealed class DialogueRunner : MonoBehaviour, IDialogueService
    {
        [SerializeField] private DialogueLineEventChannelSO _playLineRequests;
        [SerializeField] private StringEventChannelSO _onSubtitle;
        [SerializeField] private string _thoughtFormat = "<i>{0}</i>";
        [SerializeField] private string _speechFormat = "<b>{0}</b>  {1}";

        private readonly SubtitleQueue _queue = new SubtitleQueue();
        private bool _registered;

        public bool IsBusy => _queue.IsBusy;

        private void Awake()
        {
            if (ServiceLocator.TryGet<IDialogueService>(out _)) { enabled = false; return; } // duplicate _Boot
            ServiceLocator.Register<IDialogueService>(this);
            _registered = true;
            _queue.LineChanged += OnLineChanged;
        }

        private void OnEnable()
        {
            if (_playLineRequests != null) _playLineRequests.Subscribe(PlayNow);
        }

        private void OnDisable()
        {
            if (_playLineRequests != null) _playLineRequests.Unsubscribe(PlayNow);
        }

        private void OnDestroy()
        {
            if (_registered) ServiceLocator.Unregister<IDialogueService>();
        }

        private void Update() => _queue.Tick(Time.unscaledDeltaTime);

        public void PlayNow(DialogueLineSO line) => _queue.PlayNow(line);

        public void Enqueue(DialogueLineSO line) => _queue.Enqueue(line);

        public bool TryPlayAmbient(DialogueLineSO line) => _queue.TryPlayAmbient(line);

        private void OnLineChanged(DialogueLineSO line)
        {
            if (_onSubtitle == null) return;
            if (line == null) { _onSubtitle.Raise(string.Empty); return; }
            _onSubtitle.Raise(line.IsThought || string.IsNullOrEmpty(line.SpeakerName)
                ? string.Format(_thoughtFormat, line.Text)
                : string.Format(_speechFormat, line.SpeakerName, line.Text));
        }
    }
}
