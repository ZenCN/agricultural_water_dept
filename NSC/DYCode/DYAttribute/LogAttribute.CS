using System.Web.Mvc;
using System.Linq;
using System.Web;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Dynashety.DYAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class LogFilter : FilterAttribute, IActionFilter
    {
        //public enum EventType { insert = 1, update, delete, post, get, put, set }
        //private EventType e { get; set; }
        //private static string t { get; set; }
        //private static string w { get; set; }

        //public LogFilter(EventType eventType)
        //{
        //    e = eventType;
        //}

        //public static void setlogText(string text)
        //{
        //    t = text;
        //}

        //public static void setwxId(string wxId)
        //{
        //    w = wxId;
        //}

        void IActionFilter.OnActionExecuting(ActionExecutingContext AEC)
        {

        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext AEC)
        {
            //var db = new WX_DY_SERVER.Models.WXDBEntities();
            //db.LogInfo.AddObject(new WX_DY_SERVER.Models.LogInfo
            //{
            //    LogIp = GetRealIP(),
            //    LogEvent = (int)e,
            //    LogText = t,
            //    LogTime = DateTime.Now,
            //    wxId = w
            //});
            //db.SaveChanges();
        }
    }
}