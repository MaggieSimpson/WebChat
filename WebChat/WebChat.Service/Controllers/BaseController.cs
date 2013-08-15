using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Providers.Entities;
using WebChat.Service.Models;

namespace WebChat.Service.Controllers
{
    public class BaseController : ApiController
    {
        protected static Random random = new Random();

        protected readonly UnitOfWork unitOfWork = null;

        public BaseController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected HttpResponseMessage PerformOperation(Action action)
        {
            action();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        protected HttpResponseMessage PerformOperation<T>(Func<T> action)
        {
            action();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}