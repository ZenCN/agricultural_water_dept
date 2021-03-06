﻿using System.Collections.Generic;
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
    [PermissionFilter(Level = new int[] { 3 })]
    public class DT03Controller : Controller
    {
        public ActionResult Index(int year, string name)
        {
            return Json(new Other().Index(year, name, Table.DT03), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpLoad(string list, List<HttpPostedFileBase> files)
        {
            //return Json(new FileOper(new UpLoad_DataBase(Table.DT03, Session[ConstInfo.sys_session_info] as SX02_USER)).UpLoad(new JavaScriptSerializer().Deserialize<List<UFileInfo>>(list ?? string.Empty), files), JsonRequestBehavior.AllowGet);
            if (files != null)
            {
                var file_infos = new JavaScriptSerializer().Deserialize<List<UFileInfo>>(list ?? string.Empty);
                if (file_infos != null)
                {
                    file_infos.ForEach(info =>
                    {
                        files.ForEach(file =>
                        {
                            if (info.fileName == file.FileName) //删除已存在的附件
                            {
                                files.Remove(file);
                            }
                        });

                    });

                    if (files.Count == 0)
                    {
                        return null;
                    }
                }

                return
                    Json(
                        new FileOper(new UpLoad_DataBase(Table.DT03,
                            Session[ConstInfo.sys_session_info] as SX02_USER))
                            .UpLoad(file_infos, files), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public void DownLoad(string list)
        {
            new FileOper().DownLoad(new Other().DownLoad(new JavaScriptSerializer().Deserialize<int[]>(list), Table.DT03), HttpContext);
        }

        public JsonResult Search(int year, string name)
        {
            return Json(new Other().Serach(year, name, Table.DT03), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int index)
        {
            return Json(new Other().Delete(index, Table.DT03), JsonRequestBehavior.AllowGet);
        }
    }
}
