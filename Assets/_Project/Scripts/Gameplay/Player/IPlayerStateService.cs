using HollowChains.Core.Services;

namespace HollowChains.Gameplay.Player
{
    /// <summary>
    /// Persistent player state in _Boot (HP, Humanity, carried item). Each content scene has its own Player object,
    /// which restores itself from here on Start and reports changes back (PlayerStateSync).
    /// </summary>
    public interface IPlayerStateService : IService
    {
        PlayerStateData State { get; }

        void RecordHealth(int current);

        void RecordCarried(string itemId);

        void ChangeHumanity(int delta);

        /// <summary>New game: full HP, Humanity 100, empty hands.</summary>
        void ResetToNew();
    }
}
