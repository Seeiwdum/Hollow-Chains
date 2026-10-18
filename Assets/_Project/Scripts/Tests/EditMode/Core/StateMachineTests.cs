using System.Collections.Generic;
using HollowChains.Core.StateMachines;
using NUnit.Framework;

namespace HollowChains.Tests.Core
{
    public class StateMachineTests
    {
        private sealed class RecordingState : IState
        {
            private readonly string _name;
            private readonly List<string> _log;

            public RecordingState(string name, List<string> log)
            {
                _name = name;
                _log = log;
            }

            public float TickedTime { get; private set; }

            public void Enter() => _log.Add(_name + ".Enter");

            public void Tick(float deltaTime) => TickedTime += deltaTime;

            public void Exit() => _log.Add(_name + ".Exit");
        }

        [Test]
        public void ChangeState_CallsExitBeforeEnter()
        {
            var log = new List<string>();
            var fsm = new StateMachine();
            var a = new RecordingState("A", log);
            var b = new RecordingState("B", log);

            fsm.ChangeState(a);
            fsm.ChangeState(b);

            CollectionAssert.AreEqual(new[] { "A.Enter", "A.Exit", "B.Enter" }, log);
            Assert.AreSame(b, fsm.Current);
        }

        [Test]
        public void ChangeState_ToSameState_IsIgnored()
        {
            var log = new List<string>();
            var fsm = new StateMachine();
            var a = new RecordingState("A", log);

            fsm.ChangeState(a);
            fsm.ChangeState(a);

            CollectionAssert.AreEqual(new[] { "A.Enter" }, log);
        }

        [Test]
        public void Tick_ForwardsDeltaToCurrentState()
        {
            var fsm = new StateMachine();
            var a = new RecordingState("A", new List<string>());
            fsm.ChangeState(a);

            fsm.Tick(0.5f);

            Assert.AreEqual(0.5f, a.TickedTime, 1e-6f);
        }

        [Test]
        public void StateChanged_ReportsPreviousAndNext()
        {
            var fsm = new StateMachine();
            var a = new RecordingState("A", new List<string>());
            var b = new RecordingState("B", new List<string>());
            IState seenPrevious = null, seenNext = null;
            fsm.ChangeState(a);
            fsm.StateChanged += (p, n) => { seenPrevious = p; seenNext = n; };

            fsm.ChangeState(b);

            Assert.AreSame(a, seenPrevious);
            Assert.AreSame(b, seenNext);
        }
    }
}
