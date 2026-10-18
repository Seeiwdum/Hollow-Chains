using HollowChains.Core.Events;
using NUnit.Framework;
using UnityEngine;

namespace HollowChains.Tests.Core
{
    public class EventChannelTests
    {
        [Test]
        public void VoidChannel_InvokesSubscribers_AndStopsAfterUnsubscribe()
        {
            var channel = ScriptableObject.CreateInstance<VoidEventChannelSO>();
            int calls = 0;
            void Listener() => calls++;

            channel.Subscribe(Listener);
            channel.Raise();
            channel.Unsubscribe(Listener);
            channel.Raise();

            Assert.AreEqual(1, calls);
            Object.DestroyImmediate(channel);
        }

        [Test]
        public void IntChannel_PassesPayload()
        {
            var channel = ScriptableObject.CreateInstance<IntEventChannelSO>();
            int received = -1;
            channel.Subscribe(v => received = v);

            channel.Raise(1);

            Assert.AreEqual(1, received);
            Object.DestroyImmediate(channel);
        }

        [Test]
        public void Raise_WithoutSubscribers_DoesNotThrow()
        {
            var channel = ScriptableObject.CreateInstance<StringEventChannelSO>();
            Assert.DoesNotThrow(() => channel.Raise("Q001-P1"));
            Object.DestroyImmediate(channel);
        }
    }
}
