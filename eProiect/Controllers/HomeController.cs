using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace eProiect.Controllers
{

     public class HomeController : Controller
     {
          // GET: Home
         
          public ActionResult Index()
          {
              
                    return View();
              
          }
        
          public ActionResult Tables()       
          {
                return View();
          }

          public ActionResult UserProfile()
          {
                    return View();
             
          }
          public ActionResult Schedule() 
          {
                return View();
          }
  
     }
}
