using System.Web.Mvc;
using System;

namespace Dynashety.DYAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ResultFilter : FilterAttribute,IResultFilter
    {
        void IResultFilter.OnResultExecuted(ResultExecutedContext REC)
        {

        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext REC)
        {

        }
    }
}