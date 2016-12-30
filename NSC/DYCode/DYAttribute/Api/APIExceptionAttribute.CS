using System;
using System.Net;
using System.Web.Mvc;
using f = System.Web.Http.Filters;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Net.Http.Formatting;
using ElegantWM.Tools;

namespace Dynashety.DYAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class APIExceptionFilter : f.ExceptionFilterAttribute
    {
        public override void OnException(f.HttpActionExecutedContext EC)
        {
            Tool.Write(EC.Exception.Message);
            APILogFilter.setlogText(EC.Exception.Message);
            new APILogFilter(APILogFilter.EventType.log).OnActionExecuted(EC);
            EC.Response = GetResponse(new DYException { ec = "601", em = EC.Exception.Message, es = EC.Exception.StackTrace });
            //base.OnException(EC);
        }

        private HttpResponseMessage GetResponse(DYException t)
        {
            return new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new ObjectContent<DYException>(t, new JsonMediaTypeFormatter(), "application/json")
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