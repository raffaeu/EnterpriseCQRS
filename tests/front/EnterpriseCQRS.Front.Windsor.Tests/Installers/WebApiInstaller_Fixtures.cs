using System;
using System.Web.Mvc;
using Castle.Core;
using Castle.Windsor;
using EnterpriseCQRS.Front.Windsor.Components;
using EnterpriseCQRS.Front.Windsor.Installers;
using EnterpriseCQRS.Front.Windsor.Tests.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http;

namespace EnterpriseCQRS.Front.Windsor.Tests.Installers
{
    [TestClass]
    public class WebApiInstaller_Fixtures
    {
        [TestMethod]
        public void ApiInstaller_Resolve_AnyController()
        {
            Mock<HttpConfiguration> fakeConfiguration = new Mock<HttpConfiguration>();
            IWindsorContainer container = new WindsorContainer();
            container.Install(new WebApiInstaller(
                scanningAssembly:typeof(FakeHttpController).Assembly, 
                configuration:fakeConfiguration.Object));

            container.Resolve<FakeHttpController>()
                .Should().NotBeNull();
        }

        [TestMethod]
        public void ApiInstaller_Resolve_Controller_Lifestyle_Trasient()
        {
            Mock<HttpConfiguration> fakeConfiguration = new Mock<HttpConfiguration>();
            IWindsorContainer container = new WindsorContainer();
            container.Install(new WebApiInstaller(
                scanningAssembly: typeof(FakeHttpController).Assembly,
                configuration: fakeConfiguration.Object));

            container.Kernel.GetHandler(typeof(FakeHttpController))
                .ComponentModel.LifestyleType
                .Should().Be(LifestyleType.Transient);
        }

        [TestMethod]
        public void ApiInstaller_Register_DependencyResolver()
        {
            Mock<HttpConfiguration> fakeConfiguration = new Mock<HttpConfiguration>();
            //fakeConfiguration.SetupProperty(x => x.DependencyResolver);

            IWindsorContainer container = new WindsorContainer();
            container.Install(new WebApiInstaller(
                scanningAssembly: typeof(FakeHttpController).Assembly,
                configuration: fakeConfiguration.Object));

            fakeConfiguration.Object.DependencyResolver
                .Should().BeOfType<WebApiDependencyResolver>();
        }

    }
}
