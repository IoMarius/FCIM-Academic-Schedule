using eProiect.Domain.Entities.Responce;
using eProiect.Extensions;
using eProiect.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace eProiect.Controllers
{
     public class HomeController : BaseController
     {
        public ActionResult Index()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"]!="login") 
            {
                return RedirectToAction("Login", "Login");
            }

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            return View(loggedInUser); 
        }
        
        public ActionResult Tables()       
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        public ActionResult UserProfile()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            return View();         
        }

        public ActionResult Schedule() 
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData= new UserEsentialData { 
                Name=loggedInUser.Name,
                Surname=loggedInUser.Surname,
                CreatedDate=loggedInUser.CreatedDate,
                Level=loggedInUser.Level
            };
            return View(UData);
        }
     }
}
