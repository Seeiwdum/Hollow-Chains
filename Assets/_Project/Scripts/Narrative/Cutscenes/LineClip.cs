using HollowChains.Data.Dialogue;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>Timeline clip that plays a subtitle line (via EVT_PlayLine) when the playhead enters it.</summary>
    [System.Serializable]
    public sealed class LineClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private DialogueLineSO _line;
        [SerializeField] private DialogueLineEventChannelSO _playLine;

        public ClipCaps clipCaps => ClipCaps.None;

        public DialogueLineSO Line { get => _line; set => _line = value; }

        public DialogueLineEventChannelSO PlayLine { get => _playLine; set => _playLine = value; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<FireOnceBehaviour>.Create(graph);
            var line = _line;
            var channel = _playLine;
            playable.GetBehaviour().Action = () => { if (line != null && channel != null) channel.Raise(line); };
            return playable;
        }
    }
}
