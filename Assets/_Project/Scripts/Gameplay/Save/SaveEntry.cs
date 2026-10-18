using System;

namespace HollowChains.Gameplay.Save
{
    /// <summary>One system's state inside the save file.</summary>
    [Serializable]
    public sealed class SaveEntry
    {
        public string Key;
        public string Json;
    }
}
