using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace EnterpriseCQRS.Front.Windsor.Tests.Fakes
{
    public class FakeController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }
}