using HollowChains.Gameplay.Stats;
using NUnit.Framework;

namespace HollowChains.Tests.Stats
{
    public class HealthModelTests
    {
        [Test]
        public void StartsFull()
        {
            var hp = new HealthModel(10);
            Assert.AreEqual(10, hp.Current);
            Assert.AreEqual(1f, hp.Normalized, 1e-5f);
        }

        [Test]
        public void DamageAndHeal_AreClamped()
        {
            var hp = new HealthModel(10);
            hp.TakeDamage(99);
            Assert.AreEqual(0, hp.Current);
            hp.Heal(99);
            Assert.AreEqual(10, hp.Current);
        }

        [Test]
        public void SetToOne_IsCritical_LikeSceneFour()
        {
            var hp = new HealthModel(10);
            hp.SetTo(1);
            Assert.IsTrue(hp.IsCritical);
            Assert.AreEqual(0.1f, hp.Normalized, 1e-5f);
        }

        [Test]
        public void Changed_FiresOnlyOnRealChange()
        {
            var hp = new HealthModel(10);
            int calls = 0;
            hp.Changed += (_, __) => calls++;
            hp.Heal(5);      // already full → no change
            hp.TakeDamage(3);
            hp.TakeDamage(-4); // negative damage is ignored
            Assert.AreEqual(1, calls);
        }
    }
}
