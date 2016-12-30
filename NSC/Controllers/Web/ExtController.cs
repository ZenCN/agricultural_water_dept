using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSC.Controllers
{
    public class ExtController : Controller
    {
        public ActionResult FileValidate(string list)
        {
            return Json(list.Split(',').Join(new NSCEntities().SX04_SYS, a => a.ToUpper(), b => b.MD5.ToUpper(), (a, b) => a), JsonRequestBehavior.AllowGet);
        }
    }
}
