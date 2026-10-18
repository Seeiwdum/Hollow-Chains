using HollowChains.Core.Services;
using HollowChains.Gameplay.Player;
using UnityEngine;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>Loads the next content scene when the player walks in (e.g. bedroom door → S02_Astraon).</summary>
    public sealed class SceneTransitionTrigger : PlayerTriggerZone
    {
        [SerializeField] private string _targetScene;

        protected override void OnPlayerEnter(PlayerController player)
        {
            if (ServiceLocator.TryGet<ISceneFlowService>(out var flow))
            {
                player.EnterCutscene(); // freeze during the fade; the next scene has its own player
                flow.Load(_targetScene);
            }
            else
            {
                Debug.LogWarning($"{name}: no ISceneFlowService (is _Boot loaded?) — cannot load {_targetScene}.", this);
            }
        }
    }
}
