using HollowChains.Data.Dialogue;
using HollowChains.Gameplay.Player;
using UnityEngine;

namespace HollowChains.Gameplay.Triggers
{
    /// <summary>Plays a line (inner monologue, scripted remark) when the player walks in — usually with "Once" ticked.</summary>
    public sealed class LineTriggerZone : PlayerTriggerZone
    {
        [SerializeField] private DialogueLineSO _line;
        [SerializeField] private DialogueLineEventChannelSO _playLine;

        protected override void OnPlayerEnter(PlayerController player)
        {
            if (_line != null && _playLine != null) _playLine.Raise(_line);
        }
    }
}
