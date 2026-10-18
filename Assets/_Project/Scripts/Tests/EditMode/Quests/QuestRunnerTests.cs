using System.Collections.Generic;
using HollowChains.Core.Events;
using HollowChains.Data.Quests;
using HollowChains.Gameplay.Quests;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace HollowChains.Tests.Quests
{
    public class QuestRunnerTests
    {
        private readonly List<Object> _created = new List<Object>();

        [TearDown]
        public void Cleanup()
        {
            foreach (var o in _created) Object.DestroyImmediate(o);
            _created.Clear();
        }

        private T Make<T>() where T : ScriptableObject
        {
            var so = ScriptableObject.CreateInstance<T>();
            _created.Add(so);
            return so;
        }

        private static void Set(Object target, string property, object value)
        {
            var so = new SerializedObject(target);
            var p = so.FindProperty(property);
            if (value is string s) p.stringValue = s;
            else p.objectReferenceValue = (Object)value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private QuestPhaseSO Phase(string id, PhaseConditionSO condition)
        {
            var phase = Make<QuestPhaseSO>();
            Set(phase, "_id", id);
            Set(phase, "_objective", "objective " + id);
            Set(phase, "_completeWhen", condition);
            return phase;
        }

        private QuestSO Quest(params QuestPhaseSO[] phases)
        {
            var quest = Make<QuestSO>();
            Set(quest, "_id", "Q_TEST");
            var so = new SerializedObject(quest);
            var list = so.FindProperty("_phases");
            list.arraySize = phases.Length;
            for (int i = 0; i < phases.Length; i++) list.GetArrayElementAtIndex(i).objectReferenceValue = phases[i];
            so.ApplyModifiedPropertiesWithoutUndo();
            return quest;
        }

        [Test]
        public void Begin_StartsFirstPhase()
        {
            var c1 = Make<ManualConditionSO>();
            var runner = new QuestRunner(Quest(Phase("P1", c1), Phase("P2", Make<ManualConditionSO>())));
            string started = null;
            runner.PhaseStarted += p => started = p.Id;

            runner.Begin();

            Assert.AreEqual("P1", started);
            Assert.AreEqual(0, runner.State.PhaseIndex);
            Assert.AreEqual(1, c1.ActiveWatchers);
        }

        [Test]
        public void ConditionMet_AdvancesAndUnbindsPreviousPhase()
        {
            var c1 = Make<ManualConditionSO>();
            var c2 = Make<ManualConditionSO>();
            var runner = new QuestRunner(Quest(Phase("P1", c1), Phase("P2", c2)));
            runner.Begin();

            c1.Fire();

            Assert.AreEqual("P2", runner.CurrentPhase.Id);
            Assert.AreEqual(0, c1.ActiveWatchers);
            Assert.AreEqual(1, c2.ActiveWatchers);
        }

        [Test]
        public void LastPhaseMet_CompletesQuest()
        {
            var c1 = Make<ManualConditionSO>();
            var runner = new QuestRunner(Quest(Phase("P1", c1)));
            bool completed = false;
            runner.Completed += _ => completed = true;
            runner.Begin();

            c1.Fire();

            Assert.IsTrue(completed);
            Assert.IsTrue(runner.State.IsComplete);
            Assert.IsNull(runner.CurrentPhase);
        }

        [Test]
        public void FiringOldCondition_DoesNotSkipAhead()
        {
            var c1 = Make<ManualConditionSO>();
            var c2 = Make<ManualConditionSO>();
            var runner = new QuestRunner(Quest(Phase("P1", c1), Phase("P2", c2), Phase("P3", Make<ManualConditionSO>())));
            runner.Begin();
            c1.Fire();

            c1.Fire(); // the player walks back over an old trigger

            Assert.AreEqual("P2", runner.CurrentPhase.Id);
        }

        [Test]
        public void EventRaisedCondition_AdvancesOnChannelRaise()
        {
            var channel = Make<VoidEventChannelSO>();
            var condition = Make<EventRaisedConditionSO>();
            Set(condition, "_event", channel);
            var runner = new QuestRunner(Quest(Phase("P1", condition), Phase("P2", Make<ManualConditionSO>())));
            runner.Begin();

            channel.Raise();

            Assert.AreEqual("P2", runner.CurrentPhase.Id);
        }

        [Test]
        public void ResumeFromSavedState_StartsAtSavedPhase()
        {
            var quest = Quest(Phase("P1", Make<ManualConditionSO>()), Phase("P2", Make<ManualConditionSO>()));
            var runner = new QuestRunner(quest, new QuestState("Q_TEST") { PhaseIndex = 1 });
            string started = null;
            runner.PhaseStarted += p => started = p.Id;

            runner.Begin();

            Assert.AreEqual("P2", started);
        }
    }
}
