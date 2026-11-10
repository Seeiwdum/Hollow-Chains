using HollowChains.Core.Services;

namespace HollowChains.Core.Input
{
    /// <summary>Tells which input scheme the player used last ("kbm" or "gamepad", see <see cref="InputSchemes"/>).</summary>
    public interface IInputSchemeService : IService
    {
        string CurrentScheme { get; }
    }
}
