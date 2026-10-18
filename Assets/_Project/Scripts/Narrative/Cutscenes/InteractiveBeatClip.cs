using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>
    /// Pauses the cutscene until the player presses/holds Interact (`06` [IMPL]: the player must feel "I did this").
    /// The outcome is fixed — the beat cannot fail. Needs an <see cref="InteractiveBeatRunner"/> on the director.
    /// </summary>
    [System.Serializable]
    public sealed class InteractiveBeatClip : PlayableAsset, ITimelineClipAsset
    {
        public BeatKind Kind = BeatKind.Press;
        [Min(0.1f)] public float HoldSeconds = 1.5f;
        public LocalizedString Prompt = new LocalizedString();
        [Tooltip("Dev fallback when no table entry is set.")]
        public string PromptFallback = "...";
        [Tooltip("Hold beats only: prompt shown when the player chose \"press instead of hold\" (empty = use Prompt).")]
        public LocalizedString PressPrompt = new LocalizedString();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<FireOnceBehaviour>.Create(graph);
            var runner = owner != null ? owner.GetComponent<InteractiveBeatRunner>() : null;
            var clip = this;
            playable.GetBehaviour().Action = () =>
            {
                if (runner != null) runner.Begin(clip);
                else Debug.LogWarning("InteractiveBeatClip: no InteractiveBeatRunner on the director — beat skipped.");
            };
            return playable;
        }
    }
}
