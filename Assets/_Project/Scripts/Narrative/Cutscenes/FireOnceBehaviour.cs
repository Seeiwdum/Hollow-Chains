using UnityEngine;
using UnityEngine.Playables;

namespace HollowChains.Narrative.Cutscenes
{
    /// <summary>
    /// Runs an action once per graph. No reset in OnGraphStart: Pause/Resume restarts the graph and would re-fire it;
    /// PlayableDirector.Play() rebuilds the graph, so a replay still gets fresh behaviours.
    /// </summary>
    public sealed class FireOnceBehaviour : PlayableBehaviour
    {
        public System.Action Action;
        private bool _fired;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (_fired || !Application.isPlaying) return;
            _fired = true;
            Action?.Invoke();
        }
    }
}
