using eProiect.BusinessLogic;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProiect.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ISession _session;
        protected readonly IOrg _organizational;
        protected readonly IClass _class;
        protected readonly IAcademicGroup _academicGroup;
        protected readonly IClassRoom _classRoom;

        public BaseController()
        {
            var bl = new BuissinesLogic();
            _session=bl.GetSessionBL();
            _organizational=bl.GetOrgBl();
            _class=bl.GetClassBL();
            _academicGroup=bl.GetAcademicGroupBL();
            _classRoom=bl.GetClassRoomBL(); 
        }

        public void SessionStatus()
        {
            var apiCookie = Request.Cookies["X-KEY"];
            if(apiCookie != null)
            {
                var profile = _session.GetUserByCookie(apiCookie.Value);
                if(profile != null)
                {
                    System.Web.HttpContext.Current.SetMySessionObject(profile);
                    System.Web.HttpContext.Current.Session["LoginStatus"] = "login";
                }
                else
                {
                    ClearSession();
                }
            }
            else
            {
                System.Web.HttpContext.Current.Session["LoginStatus"] = "logout";
            }
        }
        public void ClearSession()
          {
            System.Web.HttpContext.Current.ClearError();
            if (ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("X-KEY"))
            {
                var cookie = ControllerContext.HttpContext.Request.Cookies["X-KEY"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
            }
            System.Web.HttpContext.Current.Session["LoginStatus"] = "logout";

          }
    }
}