using System.Web;
using System.Web.Mvc;
using f = System.Web.Http.Filters;
using System.Text.RegularExpressions;
using System;
using System.Web.Http.Controllers;
using System.Net;

namespace Dynashety.DYAttribute
{
    public class APILogFilter : f.ActionFilterAttribute
    {
        public enum EventType
        {
            reset_log = -2, reset_time, log, insert_typeinfo, update_typeinfo, delete_typeinfo, synchronous, set_material_typeinfo, find, get_typeinfo, get_news, get_list, get_time, get_log
        }

        private EventType e { get; set; }

        public APILogFilter(EventType eventType)
        {
            e = eventType;
        }

        public static void setlogText(string text)
        {
            HttpContext.Current.Session.Add("t", text);
        }

        public static void setwxId(string wxId)
        {
            HttpContext.Current.Session.Add("w", wxId);
        }

        public override void OnActionExecuting(HttpActionContext AEC)
        {

        }

        public override void OnActionExecuted(f.HttpActionExecutedContext AEC)
        {
            //var db = new WX_DY_SERVER.Models.WXDBEntities();
            //db.LogInfo.AddObject(new WX_DY_SERVER.Models.LogInfo
            //{
            //    LogIp = getIp(),
            //    LogEvent = (int)e,
            //    LogText = HttpContext.Current.Session["t"] == null ? null : HttpContext.Current.Session["t"].ToString(),
            //    LogTime = DateTime.Now,
            //    wxId = HttpContext.Current.Session["w"] == null ? null : HttpContext.Current.Session["w"].ToString()
            //});
            //db.SaveChanges();
            //HttpContext.Current.Session.Remove("w");
            //HttpContext.Current.Session.Remove("t");
        }

        public static string getIp()
        {
            try
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null || System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return string.Empty;
                string CustomerIP = string.Empty;
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                    return CustomerIP;
                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!String.IsNullOrEmpty(CustomerIP))
                    return CustomerIP;
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                else
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.Compare(CustomerIP, "unknown", true) == 0)
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                return CustomerIP;
            }
            catch (Exception e)
            {
                return string.Empty;
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 获取真ip
        /// </summary>
        /// <returns></returns>
        public string GetRealIP()
        {
            string result = String.Empty;
            result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //可能有代理 
            if (!string.IsNullOrWhiteSpace(result))
            {
                //没有"." 肯定是非IP格式
                if (result.IndexOf(".") == -1)
                {
                    result = null;
                }
                else
                {
                    //有","，估计多个代理。取第一个不是内网的IP。
                    if (result.IndexOf(",") != -1)
                    {
                        result = result.Replace(" ", string.Empty).Replace("\"", string.Empty);

                        string[] temparyip = result.Split(",;".ToCharArray());

                        if (temparyip != null && temparyip.Length > 0)
                        {
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                //找到不是内网的地址
                                if (IsIPAddress(temparyip[i]) && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];
                                }
                            }
                        }
                    }
                    //代理即是IP格式
                    else if (IsIPAddress(result))
                    {
                        return result;
                    }
                    //代理中的内容非IP
                    else
                    {
                        result = null;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        private bool IsIPAddress(string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
                return false;
            string regformat = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str);
        }
    }
}