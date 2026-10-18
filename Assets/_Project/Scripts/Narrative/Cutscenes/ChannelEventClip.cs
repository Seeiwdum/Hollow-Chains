using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Timeline clip that raises a void event channel once when the playhead enters it (HP = 1, flash, demo end…).</summary>
    [System.Serializable]
    public sealed class ChannelEventClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private VoidEventChannelSO _channel;

        public ClipCaps clipCaps => ClipCaps.None;

        public VoidEventChannelSO Channel { get => _channel; set => _channel = value; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<FireOnceBehaviour>.Create(graph);
            var channel = _channel;
            playable.GetBehaviour().Action = () => { if (channel != null) channel.Raise(); };
            return playable;
        }
    }
}
