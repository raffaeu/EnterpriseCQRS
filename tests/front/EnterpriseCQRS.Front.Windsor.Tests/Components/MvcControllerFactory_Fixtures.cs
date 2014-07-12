using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;
using Castle.Windsor;
using EnterpriseCQRS.Front.Windsor.Components;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EnterpriseCQRS.Front.Windsor.Tests.Fakes;

namespace EnterpriseCQRS.Front.Windsor.Tests.Components
{
    [TestClass]
    public class MvcControllerFactory_Fixtures
    {
        [TestMethod]
        public void Factory_Created_Requires_Container()
        {
            IWindsorContainer container = new WindsorContainer();
            MvcControllerFactory factory = new MvcControllerFactory(container.Kernel);

            IKernel expectedKernel = (IKernel)factory
                .GetType().GetField("kernel", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(factory);

            expectedKernel.ShouldBeEquivalentTo(container.Kernel);
        }

        [TestMethod]
        public void Factory_GetController_Calls_KernelResolve()
        {
            Mock<IKernel> fakeKernel = new Mock<IKernel>();
            fakeKernel.Setup(x => x.Resolve(typeof (FakeController)));
            MvcControllerFactory mockFactory = new MvcControllerFactory(fakeKernel.Object);

            MethodInfo method = mockFactory
                .GetType().GetMethod("GetControllerInstance", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(mockFactory, new object[]
            {
                new RequestContext(), typeof(FakeController)
            });

            fakeKernel.Verify(x => x.Resolve(typeof(FakeController)), Times.Exactly(1));
        }

        [TestMethod]
        public void Factory_ReleaseController_Calls_KernelRelease()
        {
            Mock<IController> fakeController = new Mock<IController>();
            Mock<IKernel> fakeKernel = new Mock<IKernel>();
            fakeKernel.Setup(x => x.ReleaseComponent(fakeController.Object));
            MvcControllerFactory mockFactory = new MvcControllerFactory(fakeKernel.Object);

            mockFactory.ReleaseController(fakeController.Object);

            fakeKernel.Verify(x => x.ReleaseComponent(fakeController.Object), Times.Exactly(1));
        }

    }
}
