using HollowChains.Data.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace HollowChains.Data.Items
{
    /// <summary>
    /// A key item (demo: the bucket ITEM_BUCKET). <see cref="CarryPrefab"/> is spawned in the player's hand when the
    /// item is restored in a new scene or from a save.
    /// </summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Items/Item", fileName = "ITEM_New")]
    public sealed class ItemSO : GameDataSO
    {
        [SerializeField] private LocalizedString _localizedName = new LocalizedString();
        [SerializeField] private string _name;
        [SerializeField] private bool _isKeyItem = true;
        [SerializeField] private GameObject _carryPrefab;

        public string DisplayName => LocalizedText.Resolve(_localizedName, _name);

        public bool IsKeyItem => _isKeyItem;

        public GameObject CarryPrefab => _carryPrefab;
    }
}
