using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using EnterpriseCQRS.Front.Windsor.Installers;
using System.Web.Mvc;
using FluentAssertions;
using System.Reflection;
using EnterpriseCQRS.Front.Windsor.Tests.Fakes;
using EnterpriseCQRS.Front.Windsor.Components;

namespace EnterpriseCQRS.Front.Windsor.Tests.Installers
{
    [TestClass]
    public class MvcInstaller_Fixtures
    {
        [TestMethod]
        public void Installer_Resolve_AnyController()
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(new MvcInstaller(typeof(FakeController).Assembly));

            container.Resolve<FakeController>()
                .Should().NotBeNull();
        }

        [TestMethod]
        public void Installer_Register_MvcFactory()
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(new MvcInstaller(typeof(FakeController).Assembly));

            ControllerBuilder.Current.GetControllerFactory()
                .Should().BeAssignableTo<MvcControllerFactory>();
        }
    }
}
