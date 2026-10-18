using System;
using System.IO;
using UnityEngine;

namespace HollowChains.Gameplay.Save
{
    /// <summary>
    /// Reads/writes one save slot as JSON. Writes go to a .tmp file first and then replace the real file, so a crash
    /// mid-write never corrupts the previous save. Plain class (folder injected) → unit-testable.
    /// </summary>
    public sealed class SaveFile
    {
        private readonly string _path;

        public SaveFile(string folder, string fileName = "save_0.json")
        {
            Directory.CreateDirectory(folder);
            _path = Path.Combine(folder, fileName);
        }

        public bool Exists => File.Exists(_path);

        public void Write(SaveData data)
        {
            string tmp = _path + ".tmp";
            File.WriteAllText(tmp, JsonUtility.ToJson(data, true));
            if (File.Exists(_path)) File.Replace(tmp, _path, null);
            else File.Move(tmp, _path);
        }

        /// <summary>The saved data, or null if missing, unreadable or from an incompatible version.</summary>
        public SaveData Read()
        {
            if (!Exists) return null;
            try
            {
                var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(_path));
                return data != null && data.SaveVersion == SaveData.CurrentVersion ? data : null;
            }
            catch (Exception e) when (e is ArgumentException || e is IOException)
            {
                Debug.LogWarning($"Save file unreadable, ignoring: {e.Message}");
                return null;
            }
        }

        public void Delete()
        {
            if (Exists) File.Delete(_path);
        }
    }
}
