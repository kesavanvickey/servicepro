using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicePro.Controllers
{
    public class MasterController : Controller
    {
        //
        // GET: /Master/
        public ActionResult Index()
        {
            Global.PageName = "Registration";
            return View();
        }
        public ActionResult PermissionDenied()
        {
            Global.PageName = "Permission Denied";
            return View();
        }
	}
}