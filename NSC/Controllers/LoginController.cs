using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dynashety.DYAttribute;
using Dynashety;

namespace NSC.Controllers
{
    //[CrossFilter]
    [ExceptionFilter]
    public class LoginController : Controller
    {
        private NSCEntities db = new NSCEntities();
        public JsonResult Login(string id, string pw)
        {
            SX02_USER SessionInfo = (Session[ConstInfo.sys_session_info] = db.SX02_USER.SingleOrDefault(t => t.ID == id && t.STATE)) as SX02_USER;
            //SessionInfo SessionInfo = (Session[ConstInfo.sys_session_info] = db.SX02_USER.Where(t => t.ID == id && t.STATE).ToList().Select(t => new SessionInfo(t)).SingleOrDefault()) as SessionInfo;
            if (SessionInfo != null)
            {
                if (SessionInfo.PW != pw) throw new Exception("对不起,密码验证失败!");
                else
                {
                    Session.Timeout = 180;
                    Response.Cookies.Add(new HttpCookie("UserName", SessionInfo.NAME)
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                    Response.Cookies.Add(new HttpCookie("AreaCode", SessionInfo.SX01_AREA.AREACODE)
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                    Response.Cookies.Add(new HttpCookie("AreaName", SessionInfo.SX01_AREA.AREANAME)
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                    Response.Cookies.Add(new HttpCookie("DeptCode", SessionInfo.SX03_DEPT.DEPTCODE)
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                    Response.Cookies.Add(new HttpCookie("DeptName", SessionInfo.SX03_DEPT.DEPTNAME)
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                    Response.Cookies.Add(new HttpCookie("Level", SessionInfo.SX01_AREA.LEVEL.ToString())
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                    return Json(new { retVal = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                throw new Exception("对不起,用户名不存在!");
        }

        [AssignSession]
        public JsonResult UpdatePW(string old_pw1, string old_pw2, string new_pw)
        {
            if (old_pw1 != old_pw2) throw new Exception("两次旧密码不一致!");
            else
            {
                if (old_pw1 == new_pw) throw new Exception("新密码和旧密码一致!");
                else
                {
                    var user = (Session[ConstInfo.sys_session_info] as SX02_USER);
                    db.SX02_USER.SingleOrDefault(t => t.ZIZOINDEX == user.ZIZOINDEX).PW = new_pw;
                    db.SaveChanges();
                    return Json(new { retVal = true }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AssignSession]
        public JsonResult LogOut()
        {
            Session.Remove(ConstInfo.sys_session_info);
            Response.Cookies.Add(new HttpCookie("UserName") { Expires = DateTime.Parse("1900-01-01 00:00:00.000") });
            Response.Cookies.Add(new HttpCookie("AreaCode") { Expires = DateTime.Parse("1900-01-01 00:00:00.000") });
            Response.Cookies.Add(new HttpCookie("AreaName") { Expires = DateTime.Parse("1900-01-01 00:00:00.000") });
            Response.Cookies.Add(new HttpCookie("DeptCode") { Expires = DateTime.Parse("1900-01-01 00:00:00.000") });
            Response.Cookies.Add(new HttpCookie("DeptName") { Expires = DateTime.Parse("1900-01-01 00:00:00.000") });
            return Json(new { retVal = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
