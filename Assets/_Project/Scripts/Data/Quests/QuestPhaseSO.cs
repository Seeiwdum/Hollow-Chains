using HollowChains.Data.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace HollowChains.Data.Quests
{
    /// <summary>One phase of a quest (e.g. "Q001-P1 หยิบถังน้ำ"). Objective text comes from the "Quests" string table.</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Quests/Quest Phase", fileName = "QP_New")]
    public sealed class QuestPhaseSO : GameDataSO
    {
        [SerializeField] private LocalizedString _localizedObjective = new LocalizedString();
        [Tooltip("Dev fallback when no table entry is set.")]
        [SerializeField, TextArea] private string _objective;
        [SerializeField] private PhaseConditionSO _completeWhen;

        public string Objective => LocalizedText.Resolve(_localizedObjective, _objective);

        public PhaseConditionSO CompleteWhen => _completeWhen;
    }
}
