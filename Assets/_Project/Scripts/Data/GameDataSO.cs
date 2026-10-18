using HollowChains.Core;
using UnityEngine;

namespace HollowChains.Data
{
    /// <summary>
    /// Base for design-data assets that save files or events refer to by a stable string ID
    /// (e.g. "Q001", "Q001-P2", "ITEM_BUCKET"). Never store runtime state on these assets.
    /// </summary>
    public abstract class GameDataSO : ScriptableObject, IIdentifiable
    {
        [SerializeField] private string _id;

        public string Id => _id;
    }
}
