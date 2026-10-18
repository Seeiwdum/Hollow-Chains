using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Runtime side of <see cref="VolumeWeightClip"/>.</summary>
    public sealed class VolumeWeightBehaviour : PlayableBehaviour
    {
        public VolumeWeightClip Clip;
        private Volume _volume;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _volume = playerData as Volume;
            if (_volume == null) return;
            double d = playable.GetDuration();
            float t = d > 0 ? Mathf.Clamp01((float)(playable.GetTime() / d)) : 1f;
            _volume.weight = Mathf.Lerp(Clip.From, Clip.To, t);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (_volume == null) return;
            double d = playable.GetDuration();
            if (d > 0 && playable.GetTime() >= d - 0.05) _volume.weight = Clip.To;
        }
    }
}
