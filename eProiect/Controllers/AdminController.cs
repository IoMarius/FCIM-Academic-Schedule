using eProiect.BusinessLogic.Interfaces;
using eProiect.Extensions;
using eProiect.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eProiect.Models;
using eProiect.Models.ViewModels;
using eProiect.Atributes;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.Academic;

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


          [UserMode(UserRole.admin)]
          public ActionResult Users()
          {
               SessionStatus();
               if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login", "Login");
               }

               GeneralViewData viewData = new GeneralViewData
               {
                    UDataList = new List<UserEsentialData>()
               };
               

               var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
               UserEsentialData UData = new UserEsentialData
               {
                    Name = loggedInUser.Name,
                    Surname = loggedInUser.Surname,
                    CreatedDate = loggedInUser.CreatedDate,
                    Level = loggedInUser.Level
               };
               viewData .UData = UData;

               var users = _session.GetAllUsers();


               foreach (var user in users.Users)
               {
                    UserEsentialData eUser = new UserEsentialData
                    {
                         Id = user.Id,
                         Name = user.Name,
                         Surname = user.Surname,
                         Email = user.Credentials.Email,
                         Level = user.Level
                    };
                    viewData.UDataList.Add(eUser);
               }

             

              
  
               return View(viewData);
          }

          [HttpPost]
          public ActionResult SaveUser(UserEsentialData userData)
          {
               // Verificăm dacă utilizatorul este autentificat
               SessionStatus();
               if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login", "Login"); // Redirecționăm către pagina de login dacă utilizatorul nu este autentificat
               }

               // Verificăm dacă datele primite sunt valide
               if (ModelState.IsValid)
               {
                    // Aici poți procesa datele primite de la client
                    // De exemplu, le poți salva în baza de date sau efectua alte operații necesare

                    // În exemplul de mai jos, presupunem că salvăm datele primite într-o variabilă de tip UserEsentialData
                    var savedUserData = userData;

                    // Returnăm un răspuns către client pentru a indica că datele au fost procesate cu succes
                    return Json(new { success = true, message = "Datele au fost salvate cu succes." });
               }
               else
               {
                    // Returnăm un răspuns de eroare către client dacă datele nu sunt valide
                    return Json(new { success = false, message = "Eroare la validarea datelor." });
               }
          }

          [UserMode(UserRole.admin)]
          public ActionResult Disciplines()
          {
               SessionStatus();
               if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login", "Login");
               }

               GeneralViewData viewData = new GeneralViewData(); 

               var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
               UserEsentialData UData = new UserEsentialData
               {
                    Name = loggedInUser.Name,
                    Surname = loggedInUser.Surname,
                    CreatedDate = loggedInUser.CreatedDate,
                    Level = loggedInUser.Level
               };
               viewData.UData = UData;

               viewData.DisciplinesDataList = _session.GetAllDisciplines();

               return View(viewData);
          }


          [UserMode(UserRole.admin)]
          public ActionResult AcademicGroups()
          {
               SessionStatus();
               if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login", "Login");
               }

               GeneralViewData viewData = new GeneralViewData();

               var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
               UserEsentialData UData = new UserEsentialData
               {
                    Name = loggedInUser.Name,
                    Surname = loggedInUser.Surname,
                    CreatedDate = loggedInUser.CreatedDate,
                    Level = loggedInUser.Level
               };
               viewData.UData = UData;

               viewData.AcademicGroupsDataList = _session.GetAllAcademicGroups();

               return View(viewData);
          }

          [UserMode(UserRole.admin)]
          public ActionResult ClassRooms()
          {
               SessionStatus();
               if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login", "Login");
               }

               GeneralViewData viewData = new GeneralViewData();

               var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
               UserEsentialData UData = new UserEsentialData
               {
                    Name = loggedInUser.Name,
                    Surname = loggedInUser.Surname,
                    CreatedDate = loggedInUser.CreatedDate,
                    Level = loggedInUser.Level
               };
               viewData.UData = UData;

               viewData.CassRoomsDataList = _session.GetAllCassRooms();

               return View(viewData);
          }
     }
}