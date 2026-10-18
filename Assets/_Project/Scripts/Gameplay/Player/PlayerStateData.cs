using System;

namespace HollowChains.Gameplay.Player
{
    /// <summary>Player state that survives scene loads and goes into the save (plain DTO).</summary>
    [Serializable]
    public sealed class PlayerStateData
    {
        /// <summary>-1 = full HP (no damage recorded yet).</summary>
        public int Health = -1;
        public int Humanity = 100;
        /// <summary>ID of the item in hand (e.g. "ITEM_BUCKET"), or empty.</summary>
        public string CarriedItemId = string.Empty;
    }
}
