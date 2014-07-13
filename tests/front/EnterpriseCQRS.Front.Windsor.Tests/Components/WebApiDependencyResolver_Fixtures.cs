using System;
using System.Linq;
using System.Web.Http.Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Castle.Windsor;
using EnterpriseCQRS.Front.Windsor.Components;
using FluentAssertions;

namespace EnterpriseCQRS.Front.Windsor.Tests.Components
{
    [TestClass]
    public class WebApiDependencyResolver_Fixtures
    {
        [TestMethod]
        public void Resolver_Implements_IDisposable()
        {
            Mock<IWindsorContainer> fakeContainer = new Mock<IWindsorContainer>();
            var mockResolver = new WebApiDependencyResolver(fakeContainer.Object) as IDisposable;

            mockResolver.Should().NotBeNull();
        }

        [TestMethod]
        public void Resolver_Requires_ValidContainer()
        {
            IWindsorContainer container = null;

            Action action = () =>
            {
                IDependencyResolver badScope =
                    new WebApiDependencyResolver(container);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Resolver_GetService_Returns_Service()
        {
            Mock<IWindsorContainer> fakeContainer = new Mock<IWindsorContainer>();
            fakeContainer.Setup(x =>
                x.Kernel.HasComponent(It.IsAny<Type>()))
                .Returns(true);
            fakeContainer.Setup(x =>
                x.Resolve(It.IsAny<Type>()))
                .Returns(new object());

            WebApiDependencyResolver mockResolver = new WebApiDependencyResolver(fakeContainer.Object);

            var expected = mockResolver.GetService(It.IsAny<Type>());
            expected.Should().NotBeNull();
        }

        [TestMethod]
        public void Resolver_GetServices_Returns_Services()
        {
            Mock<IWindsorContainer> fakeContainer = new Mock<IWindsorContainer>();
            fakeContainer.Setup(x =>
                x.ResolveAll(It.IsAny<Type>()))
                .Returns(new[] { new object(), new object() });

            WebApiDependencyResolver mockResolver = new WebApiDependencyResolver(fakeContainer.Object);

            var expected = mockResolver.GetServices(It.IsAny<Type>());
            expected.Should().NotBeNull();
            expected.ToList().Count.Should().Be(2);
        }

        [TestMethod]
        public void Resolver_BeginScope_Returns_WebApiScope()
        {
            Mock<IWindsorContainer> fakeContainer = new Mock<IWindsorContainer>();
            WebApiDependencyResolver mockResolver = new WebApiDependencyResolver(fakeContainer.Object);

            IDependencyScope expected = mockResolver.BeginScope();

            expected.Should().BeAssignableTo<WebApiDependencyScope>();
        }
    }
}
