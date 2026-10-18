using HollowChains.UI.Thai;
using NUnit.Framework;

namespace HollowChains.Tests.Thai
{
    public class ThaiMarkRulesTests
    {
        [Test]
        public void ToneAfterUpperVowel_StacksOnPrevious()
        {
            const string t = "ที่"; // ท + ี + ่
            Assert.AreEqual(ThaiMarkAction.StackOnPrevious, ThaiMarkRules.VerticalAction(t, 2));
        }

        [Test]
        public void ToneBeforeSaraAm_StacksOnNext()
        {
            const string t = "น้ำ"; // น + ้ + ำ
            Assert.AreEqual(ThaiMarkAction.StackOnNext, ThaiMarkRules.VerticalAction(t, 1));
        }

        [Test]
        public void ToneOnBareConsonant_NeedsNothing()
        {
            Assert.AreEqual(ThaiMarkAction.None, ThaiMarkRules.VerticalAction("แม่", 2));
        }

        [Test]
        public void MarksOnTallConsonant_ShiftLeft()
        {
            const string t = "ปี่"; // ป + ี + ่
            Assert.IsTrue(ThaiMarkRules.ShiftLeft(t, 1));
            Assert.IsTrue(ThaiMarkRules.ShiftLeft(t, 2));
            Assert.IsFalse(ThaiMarkRules.ShiftLeft("ที่", 1));
        }

        [Test]
        public void LowerVowelUnderDescender_Drops()
        {
            Assert.AreEqual(ThaiMarkAction.DropBelowBase, ThaiMarkRules.VerticalAction("ฎุ", 1));
            Assert.AreEqual(ThaiMarkAction.None, ThaiMarkRules.VerticalAction("ปู", 1));
        }

        [Test]
        public void BaseIndex_SkipsOverStackedMarks()
        {
            Assert.AreEqual(0, ThaiMarkRules.BaseIndex("ที่", 2));
            Assert.AreEqual(-1, ThaiMarkRules.BaseIndex("่", 0));
        }
    }
}
