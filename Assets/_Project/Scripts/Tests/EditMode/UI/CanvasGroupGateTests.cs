using HollowChains.UI.Views;
using NUnit.Framework;

namespace HollowChains.Tests.UI
{
    public class CanvasGroupGateTests
    {
        private static readonly string[] Scenes = { "S00_ComicIntro_Placeholder" };

        [Test]
        public void IsListed_ListedName_ReturnsTrue()
        {
            Assert.IsTrue(CanvasGroupGate.IsListed(Scenes, "S00_ComicIntro_Placeholder"));
        }

        [Test]
        public void IsListed_OtherName_ReturnsFalse()
        {
            Assert.IsFalse(CanvasGroupGate.IsListed(Scenes, "S01_Village"));
        }

        [Test]
        public void IsListed_NullOrEmptyInputs_ReturnFalse()
        {
            Assert.IsFalse(CanvasGroupGate.IsListed(null, "Paused"));
            Assert.IsFalse(CanvasGroupGate.IsListed(Scenes, null));
            Assert.IsFalse(CanvasGroupGate.IsListed(Scenes, ""));
            Assert.IsFalse(CanvasGroupGate.IsListed(new string[0], "Paused"));
        }

        [Test]
        public void IsListed_IsCaseSensitive()
        {
            Assert.IsFalse(CanvasGroupGate.IsListed(new[] { "Paused" }, "paused"));
        }
    }
}
