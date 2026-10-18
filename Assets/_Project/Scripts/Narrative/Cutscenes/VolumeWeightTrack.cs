using UnityEngine.Rendering;
using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Track for <see cref="VolumeWeightClip"/>s, bound to a post-processing Volume.</summary>
    [TrackColor(0.6f, 0.3f, 0.7f)]
    [TrackBindingType(typeof(Volume))]
    [TrackClipType(typeof(VolumeWeightClip))]
    public sealed class VolumeWeightTrack : TrackAsset { }
}
