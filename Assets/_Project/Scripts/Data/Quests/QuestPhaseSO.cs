using UnityEngine;

namespace HollowChains.Data.Quests
{
    /// <summary>One phase of a quest (e.g. "Q001-P1 หยิบถังน้ำ"). Objective text moves to Localization in P3.3.</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Quests/Quest Phase", fileName = "QP_New")]
    public sealed class QuestPhaseSO : GameDataSO
    {
        [SerializeField, TextArea] private string _objective;
        [SerializeField] private PhaseConditionSO _completeWhen;

        public string Objective => _objective;

        public PhaseConditionSO CompleteWhen => _completeWhen;
    }
}
