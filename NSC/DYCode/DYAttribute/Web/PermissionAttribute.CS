using System.Web.Mvc;
using System.Linq;
using System.Web;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Dynashety.DYAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class PermissionFilter : FilterAttribute, IActionFilter
    {
        public int[] Level { get; set; }
        public int PermissionInfo { get; set; }
        void IActionFilter.OnActionExecuting(ActionExecutingContext AEC)
        {
            try
            {
            }
            finally
            {

            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext AEC)
        {

        }

        private void go(string msg, ActionExecutingContext AEC)
        {
            AEC.HttpContext.Session.Remove(ConstInfo.sys_session_info);
            AEC.HttpContext.Response.StatusCode = 600;

            AEC.Result = new JsonResult()
            {
                Data = new
                {
                    ret_url = "Exception/Error",
                    ret_msg = msg
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}