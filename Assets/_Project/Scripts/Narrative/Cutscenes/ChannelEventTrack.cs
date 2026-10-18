using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Track for <see cref="ChannelEventClip"/>s (story events raised from cutscenes).</summary>
    [TrackColor(0.85f, 0.45f, 0.2f)]
    [TrackClipType(typeof(ChannelEventClip))]
    public sealed class ChannelEventTrack : TrackAsset { }
}
