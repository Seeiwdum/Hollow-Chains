using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>
    /// Moves / rotates / scales the bound Transform from A to B over the clip (eased). Used to stage capsule actors and
    /// placeholder VFX without Animator root-motion offsets (D-023). The end pose is kept after the clip.
    /// </summary>
    [System.Serializable]
    public sealed class TransformTweenClip : PlayableAsset, ITimelineClipAsset
    {
        public bool Position = true;
        public Vector3 FromPosition;
        public Vector3 ToPosition;
        public bool Rotation;
        public Vector3 FromEuler;
        public Vector3 ToEuler;
        public bool Scale;
        public Vector3 FromScale = Vector3.one;
        public Vector3 ToScale = Vector3.one;
        [Tooltip("Ease in/out (SmoothStep) instead of linear.")]
        public bool Ease = true;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TransformTweenBehaviour>.Create(graph);
            playable.GetBehaviour().Clip = this;
            return playable;
        }

        /// <summary>Applies the pose at normalized time <paramref name="t"/> (0–1).</summary>
        public void Apply(Transform target, float t)
        {
            if (target == null) return;
            float k = Ease ? Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t)) : Mathf.Clamp01(t);
            if (Position) target.position = Vector3.LerpUnclamped(FromPosition, ToPosition, k);
            if (Rotation) target.rotation = Quaternion.Euler(Vector3.LerpUnclamped(FromEuler, ToEuler, k));
            if (Scale) target.localScale = Vector3.LerpUnclamped(FromScale, ToScale, k);
        }
    }
}
