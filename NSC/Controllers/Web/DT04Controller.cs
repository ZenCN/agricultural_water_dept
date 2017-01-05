using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Dynashety;
using Dynashety.Plug;
using NSC.DYCode;

namespace NSC.Controllers.Web
{
    public class DT04Controller : Controller
    {
        public ActionResult UpLoad(string md5, List<HttpPostedFileBase> files)
        {

            return
                Json(
                    new FileOper(new UpLoad_DataBase(Table.DT04,Session[ConstInfo.sys_session_info] as SX02_USER))
                        .UpLoad(md5, files[0]), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(string json)
        {
            
        }
    }
}
