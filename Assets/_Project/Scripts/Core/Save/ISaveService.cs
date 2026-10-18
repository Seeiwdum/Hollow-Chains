using HollowChains.Core.Services;

namespace HollowChains.Core.Save
{
    /// <summary>Checkpoint save/load (one slot in the demo). Lives in _Boot.</summary>
    public interface ISaveService : IService
    {
        bool HasSave { get; }

        void Register(ISaveable saveable);

        void Unregister(ISaveable saveable);

        /// <summary>Captures every registered saveable and writes the slot. <paramref name="resumeScene"/> is where Continue starts.</summary>
        void Save(string checkpointId, string resumeScene);

        /// <summary>Restores every registered saveable from the slot; returns the scene to load, or null if there is no valid save.</summary>
        string RestoreAll();
    }
}
