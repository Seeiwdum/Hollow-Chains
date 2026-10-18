using System.Collections.Generic;
using HollowChains.Core.Events;
using HollowChains.Core.Save;
using HollowChains.Core.Services;
using UnityEngine;

namespace HollowChains.Gameplay.Save
{
    /// <summary>
    /// <see cref="ISaveService"/> in _Boot: one checkpoint slot at persistentDataPath/save_0.json.
    /// Announces whether a save exists (EVT_SaveAvailable) so the title screen can enable "Continue".
    /// Registers itself in the ServiceLocator (Awake) so saveables can register in their Start.
    /// </summary>
    public sealed class SaveService : MonoBehaviour, ISaveService
    {
        [SerializeField] private BoolEventChannelSO _onSaveAvailable;
        [SerializeField] private StringEventChannelSO _onCheckpointSaved;

        private readonly List<ISaveable> _saveables = new List<ISaveable>();
        private SaveFile _file;
        private bool _registered;

        public bool HasSave => _file.Exists;

        private void Awake()
        {
            _file = new SaveFile(Application.persistentDataPath);
            if (ServiceLocator.TryGet<ISaveService>(out _)) { enabled = false; return; } // duplicate _Boot
            ServiceLocator.Register<ISaveService>(this);
            _registered = true;
        }

        private void Start() => Announce();

        private void OnDestroy()
        {
            if (_registered) ServiceLocator.Unregister<ISaveService>();
        }

        public void Register(ISaveable saveable)
        {
            if (saveable != null && !_saveables.Contains(saveable)) _saveables.Add(saveable);
        }

        public void Unregister(ISaveable saveable) => _saveables.Remove(saveable);

        public void Save(string checkpointId, string resumeScene)
        {
            var data = new SaveData
            {
                CheckpointId = checkpointId,
                ResumeScene = resumeScene,
                SavedAtUtc = System.DateTime.UtcNow.ToString("o"),
            };
            foreach (var s in _saveables) data.Entries.Add(new SaveEntry { Key = s.SaveKey, Json = s.CaptureState() });
            _file.Write(data);
            if (_onCheckpointSaved != null) _onCheckpointSaved.Raise(checkpointId);
            Announce();
        }

        public string RestoreAll()
        {
            var data = _file.Read();
            if (data == null) return null;
            foreach (var s in _saveables)
            {
                var json = data.Find(s.SaveKey);
                if (json != null) s.RestoreState(json);
            }
            return data.ResumeScene;
        }

        private void Announce()
        {
            if (_onSaveAvailable != null) _onSaveAvailable.Raise(HasSave);
        }
    }
}
