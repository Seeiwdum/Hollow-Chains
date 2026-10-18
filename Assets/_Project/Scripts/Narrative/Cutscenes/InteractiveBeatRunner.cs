using HollowChains.Core.Events;
using HollowChains.Data.Localization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>
    /// Runs interactive beats for a PlayableDirector: freezes it, shows the prompt (EVT_InteractPrompt), waits for
    /// press / hold of the Interact action, then resumes. Hold progress is published 0–1 for an optional UI ring.
    /// Setup: on the same GameObject as the PlayableDirector; assign the Interact action and the prompt channel.
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public sealed class InteractiveBeatRunner : MonoBehaviour
    {
        [SerializeField] private InputActionReference _interact;
        [SerializeField] private StringEventChannelSO _onPrompt;
        [SerializeField] private FloatEventChannelSO _onHoldProgress;
        [Tooltip("The HUD prompt view already adds the [E] key hint.")]
        [SerializeField] private string _promptFormat = "{0}";

        private PlayableDirector _director;
        private InteractiveBeatClip _active;
        private float _held;

        public bool IsWaiting => _active != null;

        private void Awake() => _director = GetComponent<PlayableDirector>();

        private void OnEnable()
        {
            if (_interact != null) _interact.action.Enable();
        }

        public void Begin(InteractiveBeatClip clip)
        {
            _active = clip;
            _held = 0f;
            SetSpeed(0d);
            var text = LocalizedText.Resolve(clip.Prompt, clip.PromptFallback);
            if (_onPrompt != null) _onPrompt.Raise(string.Format(_promptFormat, text));
            Progress(0f);
        }

        /// <summary>Completes the current beat (also used by tests / debug skip).</summary>
        public void Complete()
        {
            if (_active == null) return;
            _active = null;
            if (_onPrompt != null) _onPrompt.Raise(string.Empty);
            Progress(0f);
            SetSpeed(1d);
        }

        // Freezing the graph (instead of PlayableDirector.Pause) keeps clips "playing", so nothing re-enters on resume.
        private void SetSpeed(double speed)
        {
            if (_director.playableGraph.IsValid()) _director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        }

        private void Update()
        {
            if (_active == null || _interact == null) return;
            if (Time.timeScale <= 0f) return; // pause menu open: E must not complete the beat behind it
            var action = _interact.action;
            if (_active.Kind == BeatKind.Press)
            {
                if (action.WasPressedThisFrame()) Complete();
                return;
            }
            _held = action.IsPressed() ? _held + Time.unscaledDeltaTime : Mathf.Max(0f, _held - Time.unscaledDeltaTime * 2f);
            Progress(_held / _active.HoldSeconds);
            if (_held >= _active.HoldSeconds) Complete();
        }

        private void Progress(float value)
        {
            if (_onHoldProgress != null) _onHoldProgress.Raise(Mathf.Clamp01(value));
        }
    }
}
