using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using WebChat.Data;
using WebChat.Service.Controllers;

namespace WebChat.Service.Models
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        private WebChatContext db = new WebChatContext();

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
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
            //db.Dispose();
        }
    }
}