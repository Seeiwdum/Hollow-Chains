using System.Collections.Generic;
using HollowChains.Data.Dialogue;
using HollowChains.Narrative.Dialogue;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace HollowChains.Tests.Dialogue
{
    public class SubtitleQueueTests
    {
        private readonly List<Object> _created = new List<Object>();

        [TearDown]
        public void Cleanup()
        {
            foreach (var o in _created) Object.DestroyImmediate(o);
            _created.Clear();
        }

        private DialogueLineSO Line(string text, float duration)
        {
            var line = ScriptableObject.CreateInstance<DialogueLineSO>();
            _created.Add(line);
            var so = new SerializedObject(line);
            so.FindProperty("_text").stringValue = text;
            so.FindProperty("_duration").floatValue = duration;
            so.ApplyModifiedPropertiesWithoutUndo();
            return line;
        }

        [Test]
        public void Enqueue_PlaysLinesInOrder_ThenClears()
        {
            var q = new SubtitleQueue();
            var seen = new List<string>();
            q.LineChanged += l => seen.Add(l != null ? l.Text : "<clear>");

            q.Enqueue(Line("a", 1f));
            q.Enqueue(Line("b", 1f));
            q.Tick(1.01f);
            q.Tick(1.01f);

            CollectionAssert.AreEqual(new[] { "a", "b", "<clear>" }, seen);
            Assert.IsFalse(q.IsBusy);
        }

        [Test]
        public void PlayNow_InterruptsAndDropsPending()
        {
            var q = new SubtitleQueue();
            q.Enqueue(Line("a", 1f));
            q.Enqueue(Line("b", 1f));

            q.PlayNow(Line("story", 1f));
            q.Tick(1.01f);

            Assert.IsFalse(q.IsBusy); // "b" was dropped
        }

        [Test]
        public void Ambient_IsRejectedWhileBusy()
        {
            var q = new SubtitleQueue();
            q.PlayNow(Line("story", 2f));

            Assert.IsFalse(q.TryPlayAmbient(Line("bark", 1f)));
            q.Tick(2.01f);
            Assert.IsTrue(q.TryPlayAmbient(Line("bark", 1f)));
        }

        [Test]
        public void AutoDuration_IsClampedBetweenTwoAndSixSeconds()
        {
            Assert.AreEqual(2f, Line("hi", 0f).Duration, 1e-4f);
            Assert.AreEqual(6f, Line(new string('x', 500), 0f).Duration, 1e-4f);
        }
    }
}
