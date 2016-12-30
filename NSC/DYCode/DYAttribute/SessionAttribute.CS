using System.Web.Mvc;
using System.Linq;
using System.Web;
using System;
using System.IO;
using System.Text.RegularExpressions;
using NSC;

namespace Dynashety.DYAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AssignSessionAttribute : FilterAttribute, IActionFilter
    {
        public string Uri { get; set; }
        public ConstInfo.AssignSession Method { get; set; }

        void IActionFilter.OnActionExecuting(ActionExecutingContext AEC)
        {
            if (AEC.HttpContext.Session[ConstInfo.sys_session_info] as SX02_USER == null)
            {
                AEC.HttpContext.Response.StatusCode = 601;
                if (Method == Dynashety.ConstInfo.AssignSession.ActionResult)
                {
                    AEC.Result = new RedirectResult(Uri);
                }
                if (Method == Dynashety.ConstInfo.AssignSession.JsonResult)
                {
                    AEC.Result = new JsonResult
                    {
                        Data = new { ret_url = Uri },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext AEC)
        {

        }
    }
}