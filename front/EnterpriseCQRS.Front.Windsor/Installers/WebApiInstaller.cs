using System.Reflection;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http;
using EnterpriseCQRS.Front.Windsor.Components;

namespace EnterpriseCQRS.Front.Windsor.Installers
{
    public class WebApiInstaller : IWindsorInstaller
    {
        private readonly Assembly scanningAssembly;
        private readonly HttpConfiguration configuration;

        public WebApiInstaller(Assembly scanningAssembly, HttpConfiguration configuration)
        {
            this.scanningAssembly = scanningAssembly;
            this.configuration = configuration;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // register all the controllers of type MVC within an assembly
            // controllers must be Transient and not PerWebRequest or multiple requests
            // over the same controller may raise some race conditions over the same session
            container.Register(Classes
                .FromAssembly(scanningAssembly)
                .BasedOn<IHttpController>()
                .LifestyleTransient());

            configuration.DependencyResolver = new WebApiDependencyResolver(container);
        }
    }
}
