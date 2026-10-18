namespace HollowChains.Core.Save
{
    /// <summary>
    /// Anything whose state goes into the save file. Each system serializes its own plain state class (JSON), so the
    /// save service never needs to know concrete types (D-020).
    /// </summary>
    public interface ISaveable
    {
        /// <summary>Stable key in the save file, e.g. "quests", "player".</summary>
        string SaveKey { get; }

        string CaptureState();

        void RestoreState(string json);
    }
}
