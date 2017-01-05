using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Dynashety;
using Dynashety.Plug;
using NSC.DYCode;
using NSC.ZJJCode;

namespace NSC.Controllers.Web
{
    public class DT04Controller : Controller
    {
        public ActionResult UpLoad(string md5)
        {

            return
                Json(new FileOper(new UpLoad_DataBase(Table.DT04, Session[ConstInfo.sys_session_info] as SX02_USER))
                        .UpLoad(md5, Request.Files[0]), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(string json)
        {
            return Json(new Handle().Save(json), JsonRequestBehavior.AllowGet);
        }
    }
}
