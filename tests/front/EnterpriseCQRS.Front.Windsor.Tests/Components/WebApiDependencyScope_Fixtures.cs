using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnterpriseCQRS.Front.Windsor.Components;
using FluentAssertions;
using Castle.Windsor;
using System.Web.Http.Dependencies;
using Moq;
using System.Linq;

namespace EnterpriseCQRS.Front.Windsor.Tests.Components
{
    [TestClass]
    public class WebApiDependencyScope_Fixtures
    {
        [TestMethod]
        public void Scope_Implements_IDisposable()
        {
            Mock<IWindsorContainer> fakeContainer = new Mock<IWindsorContainer>();
            var mockScope = new WebApiDependencyScope(fakeContainer.Object) as IDisposable;

            mockScope.Should().NotBeNull();
        }

        [TestMethod]
        public void Scope_Requires_ValidContainer()
        {
            IWindsorContainer container = null;

            Action action = () =>
            {
                IDependencyScope badScope =
                    new WebApiDependencyScope(container);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Scope_GetService_Returns_Service()
        {
            Mock<IWindsorContainer> fakeContainer = new Mock<IWindsorContainer>();
            fakeContainer.Setup(x => 
                x.Kernel.HasComponent(It.IsAny<Type>()))
                .Returns(true);
            fakeContainer.Setup(x => 
                x.Resolve(It.IsAny<Type>()))
                .Returns(new object());

            WebApiDependencyScope mockScope = new WebApiDependencyScope(fakeContainer.Object);

            var expected = mockScope.GetService(It.IsAny<Type>());
            expected.Should().NotBeNull();
        }

        [TestMethod]
        public void Scope_GetServices_Returns_Services()
        {
            Mock<IWindsorContainer> fakeContainer = new Mock<IWindsorContainer>();
            fakeContainer.Setup(x => 
                x.ResolveAll(It.IsAny<Type>()))
                .Returns(new[]{new object(), new object()});

            WebApiDependencyScope mockScope = new WebApiDependencyScope(fakeContainer.Object);

            var expected = mockScope.GetServices(It.IsAny<Type>());
            expected.Should().NotBeNull();
            expected.ToList().Count.Should().Be(2);
        }
    }
}
