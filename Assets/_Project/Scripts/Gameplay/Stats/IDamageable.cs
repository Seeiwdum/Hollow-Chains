namespace HollowChains.Gameplay.Stats
{
    /// <summary>Anything that can lose HP.</summary>
    public interface IDamageable
    {
        void TakeDamage(int amount);
    }
}
