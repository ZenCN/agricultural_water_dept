using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NSC.DYCode;
using Dynashety.Plug;
using Dynashety;
using Dynashety.DYAttribute;
using System.Web.Script.Serialization;

namespace NSC.Controllers
{
    [LogFilter]
    //[AssignSession]
    [ExceptionFilter]
    [PermissionFilter(Level = new int[] { 2 })]
    public class DT02Controller : Controller
    {
        public ActionResult Index()
        {
            return Json(new NSCEntities().DT02, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpLoad(string list, List<HttpPostedFileBase> files)
        {
            return Json(new FileOper(new UpLoad_DataBase(Table.DT02, Session[ConstInfo.sys_session_info] as SX02_USER)).UpLoad(new JavaScriptSerializer().Deserialize<List<UFileInfo>>(list ?? string.Empty), files), JsonRequestBehavior.AllowGet);
        }

        public void DownLoad(string list)
        {
            new FileOper().DownLoad(new JavaScriptSerializer().Deserialize<List<DFileInfo>>(list), HttpContext);
        }
    }
}
