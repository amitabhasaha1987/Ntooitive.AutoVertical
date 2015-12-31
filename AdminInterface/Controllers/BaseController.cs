using Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminInterface.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        protected new virtual CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
	}
}