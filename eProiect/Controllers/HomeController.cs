
using eProiect.Atributes;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using eProiect.Extensions;
using eProiect.Models.Enums;
using eProiect.Models.Users;
using eProiect.Models.ViewModels;
using eProiect.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using eProiect.Models.Schedule;

namespace eProiect.Controllers
{
     public class HomeController : BaseController
     {

        private readonly ISession _session;
        public HomeController()
        {
            var bl=new BusinessLogic.BuissinesLogic();
            _session=bl.GetSessionBL();
        }
    

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

          /*[UserMode(UserRole.admin, UserRole.teacher)]*/
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



            Models.Users.UserSchedule userSchedule = new Models.Users.UserSchedule();
            var currentSchedule = _session.GetScheduleById(loggedInUser.Id);

            //var thingy = currentSchedule.Schedule[0, 3].Item1.Discipline;
       
            if(currentSchedule!= null)
            {
                //transfering on tho another.... not optimized at all...
                for (int row = 0; row < 6; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        var currentSchedEven = currentSchedule.Schedule[row, col].Item1;
                        var currentSchedOdd = currentSchedule.Schedule[row, col].Item2;
                        //System.Diagnostics.Debug.WriteLine($"[{row},{col}]{currentSchedEven.Discipline}-{currentSchedOdd.Discipline}");

                        userSchedule.Schedule[row, col] = (
                            new Lesson
                            {
                                Discipline =    currentSchedEven.Discipline,
                                ShortName =     currentSchedOdd.ShortName,
                                Type=           currentSchedEven.Type,
                                StartTime =     currentSchedEven.StartTime,
                                EndTime=        currentSchedEven.EndTime,
                                WeekDay =       currentSchedEven.WeekDay,
                                Classroom =     currentSchedEven.Classroom,
                                AcademicGroup=  currentSchedEven.GroupName,
                                LessonLength=   new LessonLength(currentSchedEven.LessonLength.GetLength()),               
                                WeekSpan=       (LessonWeekType)currentSchedEven.WeekSpan
                            },
                            new Lesson
                            {
                                Discipline =    currentSchedOdd.Discipline,
                                ShortName =     currentSchedOdd.ShortName,
                                Type =          currentSchedOdd.Type,
                                StartTime =     currentSchedOdd.StartTime,
                                EndTime =       currentSchedOdd.EndTime,
                                WeekDay =       currentSchedOdd.WeekDay,
                                Classroom =     currentSchedOdd.Classroom,
                                AcademicGroup = currentSchedOdd.GroupName,
                                LessonLength =  new LessonLength(currentSchedOdd.LessonLength.GetLength()),
                                WeekSpan =      (LessonWeekType)currentSchedOdd.WeekSpan
                            }
                        );
                    }
                }
            }
            

            ScheduleViewData viewData = new ScheduleViewData
            {
                Schedule = userSchedule,
                UData = UData
            };

            return View(viewData);
        }
          
        public ActionResult AddLesson()
        {
            var ClassInfo = Request.QueryString["linfo"];

            var splitData= ClassInfo.Split('|');
            string time= splitData[0];
            int day = int.Parse(splitData[1]);

            var splitTime = time.Split(':');
            int hour= int.Parse(splitTime[0]);
            int minutes = int.Parse(splitTime[1]);

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            ReducedUser UData = new ReducedUser
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };

            ScheduleViewData ViewData = new ScheduleViewData
            {
                ClassInfo = new SelectedClassInfo(new TimeSpan(hour, minutes, 0), day),
                UData=new UserEsentialData
                {
                    Name=UData.Name,
                    Surname=UData.Surname,
                    CreatedDate=UData.CreatedDate,
                    Level= UData.Level
                }
            };

            return View(ViewData);
        }

        [HttpPost]
        public ActionResult AddLesson(string lessonInfo)
        {
            return RedirectToAction("AddLesson", "Home", new {@linfo=lessonInfo});
        }

        [UserMode(UserRole.admin, UserRole.teacher)]
          public ActionResult Logout()
          {
               ClearSession();
               return RedirectToAction("Login", "Login");
          }
     }
}
