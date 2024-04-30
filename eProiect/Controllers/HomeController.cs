
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
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.User.DBModel;
using System.Configuration;
using System.Text.RegularExpressions;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.Schedule;

namespace eProiect.Controllers
{
     public class HomeController : BaseController
     {
        public HomeController()
        { 
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

        [HttpGet]
        public ActionResult GetLoggedUserDisciplineTypes(int disciplineId)
        {
            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            var classTypes = _organizational.GetTypesByDisciplineForUser(disciplineId, loggedInUser.Id);
            
            return Json(classTypes, JsonRequestBehavior.AllowGet);
        }

        [UserMode(UserRole.admin, UserRole.teacher)]
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
            var currentSchedule = _organizational.GetScheduleById(loggedInUser.Id);

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

                        if (currentSchedOdd.Type==null) {
                            userSchedule.Schedule[row, col] = (
                                new Lesson
                                {
                                    Discipline = currentSchedEven.Discipline,
                                    ShortName = currentSchedEven.ShortName,
                                    Type = currentSchedEven.Type,
                                    StartTime = currentSchedEven.StartTime,
                                    EndTime = currentSchedEven.EndTime,
                                    WeekDay = currentSchedEven.WeekDay,
                                    Classroom = currentSchedEven.Classroom,
                                    AcademicGroup = currentSchedEven.GroupName,
                                    LessonLength = new LessonLength(currentSchedEven.LessonLength.GetLength()),
                                    WeekSpan = (LessonWeekType)currentSchedEven.WeekSpan
                                },
                                new Lesson
                                {
                                    Discipline = "NULL",
                                    ShortName = "NULL",
                                    Type = "NULL",
                                    StartTime = new TimeSpan(0, 0, 0),
                                    EndTime = new TimeSpan(0, 0, 0),
                                    WeekDay = "NULL",
                                    Classroom = "NULL",
                                    AcademicGroup = "NULL",
                                    LessonLength = new LessonLength(),
                                    WeekSpan = (LessonWeekType.ODD)
                                }
                            );
                        } 
                        else if (currentSchedEven.Type == null)
                        {
                            userSchedule.Schedule[row, col] = (
                                new Lesson
                                {
                                    Discipline = "NULL",
                                    ShortName = "NULL",
                                    Type = "NULL",
                                    StartTime = new TimeSpan(0, 0, 0),
                                    EndTime = new TimeSpan(0, 0, 0),
                                    WeekDay = "NULL",
                                    Classroom = "NULL",
                                    AcademicGroup = "NULL",
                                    LessonLength = new LessonLength(),
                                    WeekSpan = (LessonWeekType.ODD)
                                },
                                new Lesson
                                {
                                    Discipline = currentSchedOdd.Discipline,
                                    ShortName = currentSchedOdd.ShortName,
                                    Type = currentSchedOdd.Type,
                                    StartTime = currentSchedOdd.StartTime,
                                    EndTime = currentSchedOdd.EndTime,
                                    WeekDay = currentSchedOdd.WeekDay,
                                    Classroom = currentSchedOdd.Classroom,
                                    AcademicGroup = currentSchedOdd.GroupName,
                                    LessonLength = new LessonLength(currentSchedOdd.LessonLength.GetLength()),
                                    WeekSpan = (LessonWeekType)currentSchedOdd.WeekSpan
                                }
                            );
                        }
                        else
                        {
                            userSchedule.Schedule[row, col] = (
                                new Lesson
                                {
                                    Discipline = currentSchedEven.Discipline,
                                    ShortName = currentSchedEven.ShortName,
                                    Type = currentSchedEven.Type,
                                    StartTime = currentSchedEven.StartTime,
                                    EndTime = currentSchedEven.EndTime,
                                    WeekDay = currentSchedEven.WeekDay,
                                    Classroom = currentSchedEven.Classroom,
                                    AcademicGroup = currentSchedEven.GroupName,
                                    LessonLength = new LessonLength(currentSchedEven.LessonLength.GetLength()),
                                    WeekSpan = (LessonWeekType)currentSchedEven.WeekSpan
                                },
                                new Lesson
                                {
                                    Discipline = currentSchedOdd.Discipline,
                                    ShortName = currentSchedOdd.ShortName,
                                    Type = currentSchedOdd.Type,
                                    StartTime = currentSchedOdd.StartTime,
                                    EndTime = currentSchedOdd.EndTime,
                                    WeekDay = currentSchedOdd.WeekDay,
                                    Classroom = currentSchedOdd.Classroom,
                                    AcademicGroup = currentSchedOdd.GroupName,
                                    LessonLength = new LessonLength(currentSchedOdd.LessonLength.GetLength()),
                                    WeekSpan = (LessonWeekType)currentSchedOdd.WeekSpan
                                }
                            );
                        }

                    }
                }
            }
            

            GeneralViewData viewData = new GeneralViewData
            {
                Schedule = userSchedule,
                UData = UData
            };

            return View(viewData);
        }

        [HttpPost]
        public ActionResult AddNewClass(ComposedClassInfo composedData, List<int> groupIds)
        {
            //check for negatives!!!!!!!!!!!!!!
            var properties = composedData.GetType().GetProperties();
            foreach(var property in properties)
            {
                if(property.PropertyType== typeof(int))
                {
                    var propValue = property.GetValue(composedData);
                    if(Convert.ToDouble(propValue) < 0)
                    {
                        var responseList = new List<ActionResponse>
                        {
                            new ActionResponse()
                            {
                                Status = false,
                                ActionStatusMsg = "Nu au fost selectati toti parametrii.",
                            }
                        };
                        return Json(responseList);
                    }
                }
            }


            TimeSpan startTime = new TimeSpan(composedData.Hours, composedData.Minutes, 0);
            TimeSpan endTime;
            if (composedData.Span == 1)
            {
                endTime = startTime + new TimeSpan(1, 30, 0);
            }
            else
            {
                if (startTime == new TimeSpan(11, 30, 0))
                    endTime = startTime + new TimeSpan(3, 30, 0);
                else
                    endTime = startTime + new TimeSpan(3, 15, 0);
            }

            //get user 
            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();

            //goteeem!!!
            //list if some groups are busy
            List<ActionResponse> responses = new List<ActionResponse>();

            foreach(var groupId in groupIds)
            {
                var newClass = new Class
                {
                    UserDiscipline = new UserDiscipline{ 
                        Id=composedData.UserDisciplineId,
                        ClassTypeId = composedData.TypeId,
                        Type = new ClassType {Id = composedData.TypeId},
                        
                        //add user
                        UserId = loggedInUser.Id,
                        User = new User { Id=loggedInUser.Id}
                    },
                    AcademicGroup  = new AcademicGroup {   Id = groupId , Year=composedData.Year}, 
                    ClassRoom      = new ClassRoom{ Id = composedData.ClassroomId }, 
                    WeekDay        = new WeekDay{Id=composedData.Day+1},
                    StartTime      = startTime,
                    EndTime        = endTime,
                    Frequency      = (Domain.Entities.Academic.ClassFrequency)composedData.Frequency,
                    
                };

                var result = _organizational.AddNewClass(newClass);
                responses.Add(result); //NOT WORKING 
            }

            return Json(responses);
        }
        
        [HttpPost]
        public ActionResult GetOptionsByYear(int year)
        {
            List<AcademicGroup> groupList = new List<AcademicGroup>();

            if (year == 0)
            {
                return Json(groupList);
            }

            groupList = _organizational.GetAcadGroupsList(year);
            return Json(groupList);
        }


        [HttpGet]
        public ActionResult GetLoggedUserDisciplines()
        {
            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            if (loggedInUser == null)
                return Json(new List<Discipline>());

            var discList=_organizational.GetDisciplinesById(loggedInUser.Id);
            return Json(discList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetFreeClassroomsByFloor(FreeClassroomsMinimalRequest requestData)
        {
            /* var freeClassrooms = _organizational.GetFreeClassroomsByFloor(floor);
             return Json(freeClassrooms, JsonRequestBehavior.AllowGet);*/
            var freeClassrooms = _organizational.GetFreeClassroomsByFloorAndTime(
                    new FreeClassroomsRequest()
                    {
                        Floor=requestData.Floor,
                        WeekdayId=requestData.WeekdayId+1,
                        Span=requestData.Span,
                        StartTime= new TimeSpan(requestData.StartHour, requestData.StartMinute, 0),
                        Frequency= (Domain.Entities.Academic.ClassFrequency)requestData.Frequency
                    }
                );

            return Json(freeClassrooms, JsonRequestBehavior.AllowGet);
        }

        [UserMode(UserRole.admin, UserRole.teacher)]
         public ActionResult Logout()
          {
               ClearSession();
               return RedirectToAction("Login", "Login");
          }
     }
}
