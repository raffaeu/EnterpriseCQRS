using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EnterpriseCQRS.Front.Windsor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EnterpriseCQRS.Front.Windsor.Installers
{
    public class MvcInstaller : IWindsorInstaller
    {
        private readonly Assembly scanningAssembly;

        public MvcInstaller(Assembly scanningAssembly)
        {
            this.scanningAssembly = scanningAssembly;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // register all the controllers of type MVC within an assembly
            container.Register(Classes
                .FromAssembly(scanningAssembly)
                .BasedOn<IController>()
                .LifestyleTransient());
                
            // setup the custom Mvc controller factory
            var controllerFactory = new MvcControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}
