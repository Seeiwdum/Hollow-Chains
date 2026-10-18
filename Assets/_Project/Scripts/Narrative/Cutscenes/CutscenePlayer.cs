using HollowChains.Core.Events;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>
    /// Starts a PlayableDirector when a story event fires (e.g. EVT_ArenaReached → SC-04..06) and announces start /
    /// end so gameplay can lock and hide the player. Plays once per scene load.
    /// Setup: on the director object; set Play On, Started, Finished channels. Director: Play On Awake off.
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public sealed class CutscenePlayer : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _playOn;
        [SerializeField] private VoidEventChannelSO _onStarted;
        [SerializeField] private VoidEventChannelSO _onFinished;

        private PlayableDirector _director;
        private bool _played;

        private void Awake()
        {
            _director = GetComponent<PlayableDirector>();
            _director.playOnAwake = false;
        }

        private void OnEnable()
        {
            if (_playOn != null) _playOn.Subscribe(Play);
            _director.stopped += OnStopped;
        }

        private void OnDisable()
        {
            if (_playOn != null) _playOn.Unsubscribe(Play);
            if (_director != null) _director.stopped -= OnStopped;
        }

        public void Play()
        {
            if (_played) return;
            _played = true;
            if (_onStarted != null) _onStarted.Raise();
            BindCameraBrain();
            _director.time = 0;
            _director.Play();
        }

        /// <summary>The CinemachineBrain lives in _Boot (another scene), so Cinemachine tracks are bound at runtime.</summary>
        private void BindCameraBrain()
        {
            var brain = Camera.main != null ? Camera.main.GetComponent<CinemachineBrain>() : null;
            if (brain == null || _director.playableAsset == null) return;
            foreach (var output in _director.playableAsset.outputs)
                if (output.outputTargetType == typeof(CinemachineBrain)) _director.SetGenericBinding(output.sourceObject, brain);
        }

        private void OnStopped(PlayableDirector _)
        {
            if (_onFinished != null) _onFinished.Raise();
        }
    }
}
