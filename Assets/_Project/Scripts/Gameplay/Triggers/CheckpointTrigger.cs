using HollowChains.Core.Save;
using HollowChains.Core.Services;
using HollowChains.Gameplay.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>
    /// Saves the game when the player walks in (GDD: CP#1 leaving the bedroom, CP#2 at the village gate).
    /// "Continue" then loads <see cref="_resumeScene"/> (empty = this scene) from its start.
    /// </summary>
    public sealed class CheckpointTrigger : PlayerTriggerZone
    {
        [SerializeField] private string _checkpointId = "CP_01";
        [Tooltip("Scene that Continue loads. Empty = the scene this trigger is in.")]
        [SerializeField] private string _resumeScene;

        protected override void OnPlayerEnter(PlayerController player)
        {
            if (!ServiceLocator.TryGet<ISaveService>(out var save)) return;
            save.Save(_checkpointId, string.IsNullOrEmpty(_resumeScene) ? gameObject.scene.name : _resumeScene);
        }
    }
}
