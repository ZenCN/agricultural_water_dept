using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return File("~/client/index.html", "text/html");
        }
    }
}
