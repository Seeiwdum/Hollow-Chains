using UnityEngine;
using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Track for <see cref="TransformTweenClip"/>s, bound to the Transform it moves.</summary>
    [TrackColor(0.4f, 0.7f, 0.9f)]
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(TransformTweenClip))]
    public sealed class TransformTweenTrack : TrackAsset { }
}
