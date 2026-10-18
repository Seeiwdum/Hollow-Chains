using System;
using System.Collections.Generic;

namespace HollowChains.Gameplay.Save
{
    /// <summary>What is written to save_0.json (plain DTO for JsonUtility).</summary>
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentVersion = 1;

        public int SaveVersion = CurrentVersion;
        public string CheckpointId;
        public string ResumeScene;
        public string SavedAtUtc;
        public List<SaveEntry> Entries = new List<SaveEntry>();

        public string Find(string key)
        {
            foreach (var e in Entries) if (e.Key == key) return e.Json;
            return null;
        }
    }
}
