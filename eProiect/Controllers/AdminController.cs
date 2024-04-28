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
using eProiect.Domain.Entities.User.DBModel;

namespace eProiect.Controllers
{
    public class AdminController : BaseController
    {

          private readonly IAdmin _admin;
          private readonly IDiscipline _discipline;
          private readonly IAcademicGroup _academicGroup;
          private readonly IClassRoom _classRoom;


          public AdminController()
          {
               var bl = new BusinessLogic.BuissinesLogic();
               _admin = bl.GetAdminBL();
               _academicGroup = bl.GetAcademicGroupBL();
               _classRoom = bl.GetClassRoomBL();
               _discipline = bl.GetDisciplineBL();
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

               var users = _admin.GetAllUsers();


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
          [UserMode(UserRole.admin)]
          
          public ActionResult SaveUser(UserEsentialData userData)
          {
               SessionStatus();
               if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login", "Login");
               }

               if (ModelState.IsValid)
               {
                    try
                    {
                         // Construct edited user data
                         var editedUserData = new User
                         {
                              Id = userData.Id,
                              Name = userData.Name,
                              Surname = userData.Surname,
                              Level = userData.Level,
                              Credentials = new UserCredential
                              {
                                   Id = userData.Id,
                                   Email = userData.Email
                              }
                         };

                         // Call session method to edit user
                         var response = _admin.EditUsers(editedUserData);

                         // Return success or failure message
                         return Json(new { success = response.Status, message = response.ActionStatusMsg });
                    }
                    catch (Exception ex)
                    {
                         // Handle any exceptions
                         return Json(new { success = false, message = "An error occurred while editing user data: " + ex.Message });
                    }
               }
               else
               {
                    // Return validation error message
                    return Json(new { success = false, message = "Invalid data. Please check the provided information." });
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

               viewData.DisciplinesDataList = _discipline.GetAllDisciplines();

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

               viewData.AcademicGroupsDataList = _academicGroup.GetAllAcademicGroups();

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

               viewData.CassRoomsDataList = _classRoom.GetAllCassRooms();

               return View(viewData);
          }
     }
}