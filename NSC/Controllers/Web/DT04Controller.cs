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
            var files = Request.Files;

            if (files.Count > 0)
            {
                return
                    Json(new FileOper(new UpLoad_DataBase(Table.DT04, Session[ConstInfo.sys_session_info] as SX02_USER))
                        .UpLoad(md5, Request.Files[0]), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Save(string json)
        {
            return Json(new TB04().Save(json), JsonRequestBehavior.AllowGet);
        }

        public string Modify(string json)
        {
            return new TB04().Modify(json);
        }

        public string Query(string city_code, string county_code, string station_name, int level, int state)
        {
            return new TB04().Query(city_code, county_code, station_name, level, state);
        }

        public void Download(string file_name, string file_url)
        {
            Response.AppendHeader("Content-disposition", "attachment;filename=" + file_name);
            Response.WriteFile(AppDomain.CurrentDomain.BaseDirectory + "Zizo\\" + file_url);
        }

        public string QueryStation(string key_words, int level)
        {
            return new TB04().QueryStation(key_words, level);
        }

        public string ChangeState(int id, string oper)
        {
            return new TB04().ChangeState(id, oper);
        }
    }
}
