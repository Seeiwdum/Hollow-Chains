using System;
using HollowChains.Core.Services;
using NUnit.Framework;

namespace HollowChains.Tests.Core
{
    public class ServiceLocatorTests
    {
        private interface IFakeService : IService { }

        private sealed class FakeService : IFakeService { }

        [SetUp]
        [TearDown]
        public void Reset() => ServiceLocator.Clear();

        [Test]
        public void Register_ThenGet_ReturnsSameInstance()
        {
            var service = new FakeService();
            ServiceLocator.Register<IFakeService>(service);

            Assert.AreSame(service, ServiceLocator.Get<IFakeService>());
        }

        [Test]
        public void Get_Unregistered_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => ServiceLocator.Get<IFakeService>());
        }

        [Test]
        public void Register_Twice_Throws()
        {
            ServiceLocator.Register<IFakeService>(new FakeService());
            Assert.Throws<InvalidOperationException>(() => ServiceLocator.Register<IFakeService>(new FakeService()));
        }

        [Test]
        public void TryGet_AfterUnregister_ReturnsFalse()
        {
            ServiceLocator.Register<IFakeService>(new FakeService());
            ServiceLocator.Unregister<IFakeService>();

            Assert.IsFalse(ServiceLocator.TryGet<IFakeService>(out _));
        }
    }
}
