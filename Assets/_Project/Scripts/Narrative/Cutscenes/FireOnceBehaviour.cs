using UnityEngine;
using UnityEngine.Playables;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Runs an action once per graph play — resuming a paused director (interactive beats) does not repeat it.</summary>
    public sealed class FireOnceBehaviour : PlayableBehaviour
    {
        public System.Action Action;
        private bool _fired;

        public override void OnGraphStart(Playable playable) => _fired = false;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (_fired || !Application.isPlaying) return;
            _fired = true;
            Action?.Invoke();
        }
    }
}
