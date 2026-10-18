using HollowChains.Core.Events;
using HollowChains.Data.Localization;
using HollowChains.Gameplay.Player;
using UnityEngine;
using UnityEngine.Localization;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>
    /// Shows a control hint when the player walks in (SC-01 tutorial: move, camera, quest log) — usually with "Once".
    /// Setup: trigger collider; assign a UI-table key and EVT_ControlHint (the HUD hint view auto-hides it).
    /// </summary>
    public sealed class HintTriggerZone : PlayerTriggerZone
    {
        [SerializeField] private LocalizedString _hint = new LocalizedString();
        [Tooltip("Dev fallback when no table entry is set.")]
        [SerializeField] private string _fallback = "...";
        [SerializeField] private StringEventChannelSO _onHint;

        protected override void OnPlayerEnter(PlayerController player)
        {
            if (_onHint != null) _onHint.Raise(LocalizedText.Resolve(_hint, _fallback));
        }
    }
}
