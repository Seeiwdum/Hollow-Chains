using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Track for subtitle <see cref="LineClip"/>s.</summary>
    [TrackColor(0.95f, 0.89f, 0.77f)]
    [TrackClipType(typeof(LineClip))]
    public sealed class LineTrack : TrackAsset { }
}
