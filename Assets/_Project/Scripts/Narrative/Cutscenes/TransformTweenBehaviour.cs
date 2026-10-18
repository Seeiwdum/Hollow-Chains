using UnityEngine;
using UnityEngine.Playables;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Runtime side of <see cref="TransformTweenClip"/>: evaluates the tween each frame and snaps to the end pose when the clip finishes.</summary>
    public sealed class TransformTweenBehaviour : PlayableBehaviour
    {
        public TransformTweenClip Clip;
        private Transform _target;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _target = playerData as Transform;
            double duration = playable.GetDuration();
            Clip.Apply(_target, duration > 0 ? (float)(playable.GetTime() / duration) : 1f);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // Called when the clip ends (or the director pauses): keep the end pose if we reached the end.
            if (_target == null) return;
            double duration = playable.GetDuration();
            if (duration > 0 && playable.GetTime() >= duration - 0.05) Clip.Apply(_target, 1f);
        }
    }
}
