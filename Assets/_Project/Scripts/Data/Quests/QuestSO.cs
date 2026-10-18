using System.Collections.Generic;
using UnityEngine;

namespace HollowChains.Data.Quests
{
    /// <summary>A quest told in ordered phases (`04` §2). Example: Q001 "น้ำจากลำธาร" with phases P1–P5.</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Quests/Quest", fileName = "Q_New")]
    public sealed class QuestSO : GameDataSO
    {
        [SerializeField] private string _title;
        [SerializeField] private List<QuestPhaseSO> _phases = new List<QuestPhaseSO>();

        public string Title => _title;

        public IReadOnlyList<QuestPhaseSO> Phases => _phases;
    }
}
