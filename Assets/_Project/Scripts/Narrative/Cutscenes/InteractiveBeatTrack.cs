using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Track for <see cref="InteractiveBeatClip"/>s (player input moments inside a cutscene).</summary>
    [TrackColor(0.9f, 0.75f, 0.3f)]
    [TrackClipType(typeof(InteractiveBeatClip))]
    public sealed class InteractiveBeatTrack : TrackAsset { }
}
