using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Blends the bound Volume's weight from A to B (Awakening chromatic look, ending desaturation).</summary>
    [System.Serializable]
    public sealed class VolumeWeightClip : PlayableAsset, ITimelineClipAsset
    {
        [Range(0f, 1f)] public float From;
        [Range(0f, 1f)] public float To = 1f;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<VolumeWeightBehaviour>.Create(graph);
            playable.GetBehaviour().Clip = this;
            return playable;
        }
    }

}
