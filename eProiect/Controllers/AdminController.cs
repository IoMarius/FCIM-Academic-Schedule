using eProiect.BusinessLogic.Interfaces;
using eProiect.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProiect.Controllers
{
    public class AdminController : BaseController
    {

          private readonly ISession _session;
          public AdminController()
          {
               var bl = new BusinessLogic.BuissinesLogic();
               _session = bl.GetSessionBL();
          }

          public ActionResult Index()
          {
               SessionStatus();
               if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login", "Login");
               }

               var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
               return View(loggedInUser);
          }
    }
}