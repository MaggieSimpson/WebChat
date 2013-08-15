using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using WebChat.Service.Controllers;

namespace WebChat.Service.Models
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            // This example does not support child scopes, so we simply return 'this'.
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(UserController))
            {
                return new UserController(new UnitOfWork());
            }

            if (serviceType == typeof(MessageController))
            {
                return new MessageController(new UnitOfWork());
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
        }
    }
}