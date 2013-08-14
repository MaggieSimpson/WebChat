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
    public class BaseControllerTemp : ApiController
    {
        protected readonly UnitOfWork unitOfWork = null;

        public BaseControllerTemp(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}