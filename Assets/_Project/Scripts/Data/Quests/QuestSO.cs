using System.Collections.Generic;
using HollowChains.Data.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace HollowChains.Data.Quests
{
    /// <summary>A quest told in ordered phases (`04` §2). Example: Q001 "น้ำจากลำธาร" with phases P1–P5.</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Quests/Quest", fileName = "Q_New")]
    public sealed class QuestSO : GameDataSO
    {
        [SerializeField] private LocalizedString _localizedTitle = new LocalizedString();
        [Tooltip("Dev fallback when no table entry is set.")]
        [SerializeField] private string _title;
        [SerializeField] private List<QuestPhaseSO> _phases = new List<QuestPhaseSO>();

        public string Title => LocalizedText.Resolve(_localizedTitle, _title);

        public IReadOnlyList<QuestPhaseSO> Phases => _phases;
    }
}
