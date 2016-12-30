using System;
using System.Net;
using System.Web.Mvc;

namespace Dynashety.DYAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext EC)
        {
            EC.Result = Error(EC);
            EC.ExceptionHandled = true;
        }

        protected JsonResult Error(ExceptionContext EC)
        {
            EC.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            EC.HttpContext.Response.TrySkipIisCustomErrors = true;
            return new JsonResult()
            {
                Data = new DYException() { ec = EC.HttpContext.Response.StatusCode.ToString(), em = EC.Exception.Message, es = EC.Exception.StackTrace },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public class DYException
        {
            public string ec { get; set; }
            public string em { get; set; }
            public string es { get; set; }
        }
    }
}