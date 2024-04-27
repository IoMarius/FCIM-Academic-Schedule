using AutoMapper;
using eProiect.BusinessLogic.DBContext;
using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;
using eProiect.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eProiect.BusinessLogic.Core
{
     public class UserApi
     {
        internal ActionResponse RLoginUpService(ULoginData data)
        {
            //TEST ADDING USR
            /*using (var db = new UserContext())
            {
                var newCredentials = new UserCredential
                {
                    Email = "bemol@gmail.com",
                    Password = LoginHelper.HashGen("12345678")
                };

                var newUser = new User
                {
                    Name = "Coco",
                    Surname = "Jumbo",
                    CreatedDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    LastIp = "192.168.1.1",
                    Credentials = newCredentials
                };
                db.UserCredentials.Add(newCredentials);
                db.Users.Add(newUser);
                db.SaveChanges();
            }*/
            //TEST

            User result;
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Credential))
            {
                var pass = LoginHelper.HashGen(data.Password);

                using (var db=new UserContext())
                {
                    result = db.Users.FirstOrDefault(u => u.Credentials.Email == data.Credential && u.Credentials.Password == pass);
                }

                if (result == null)
                {
                   // System.Diagnostics.Debug.WriteLine("ULoginResp returned status {FALSE}. Incorrect password or username.");
                    return new ActionResponse { Status = false, ActionStatusMsg = "The username or password is incorrect." };
                }

                using (var todo=new UserContext())
                {
                    result.LastIp = data.LoginIp;
                    result.LastLogin = data.LoginDateTime;
                    todo.Entry(result).State= EntityState.Modified;
                    todo.SaveChanges();
                }
                //System.Diagnostics.Debug.WriteLine("ULoginResp returned status {TRUE}.");
                return new ActionResponse { Status = true };
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("ULoginResp returned status {FALSE}. Invalid email address.");
                return new ActionResponse { Status = false, ActionStatusMsg = "Invalid email address." };
            }
        }

        internal HttpCookie Cookie(string loginCredential)
        {
            var apiCookie = new HttpCookie("X-KEY")
            {
                Value = CookieBaker.Create(loginCredential)
            };

            using (var db = new SessionContext()) 
            {
                Session curent;
                var validate = new EmailAddressAttribute();
                if (validate.IsValid(loginCredential))
                {
                    curent = (from e in db.Sessions where e.Email == loginCredential select e).FirstOrDefault();
                }
                else
                {
                    curent = (from e in db.Sessions where e.Email == loginCredential select e).FirstOrDefault();
                }

                if (curent != null)
                {
                    curent.CookieString = apiCookie.Value;
                    curent.ExpireTime = DateTime.Now.AddMinutes(60);
                    using (var todo = new SessionContext())
                    {
                        todo.Entry(curent).State = EntityState.Modified;
                        todo.SaveChanges();
                    }
                }
                else
                {
                    db.Sessions.Add(new Session
                    {
                        Email = loginCredential,
                        CookieString = apiCookie.Value,
                        ExpireTime = DateTime.Now.AddMinutes(60)
                    });
                    db.SaveChanges();
                }
            }

            return apiCookie;
        }
     
        internal ReducedUser UserByCookie(string cookieString){
            Session session;
            User currentUser;

            using(var db=new SessionContext())
            {
                //checking if the cookie exists in the database and it has not expired yet.
                session = db.Sessions.FirstOrDefault(s=>s.CookieString==cookieString &&s.ExpireTime>DateTime.Now);
            }

            if (session == null)
                return null;

            using (var db=new UserContext())
            {
                //query existing users and identify to which the email belongs from the cookies table.
                currentUser = db.Users.FirstOrDefault(u => u.Credentials.Email == session.Email);
            }

            if(currentUser==null) 
                return null;

            //use mapper, but for now no mapper :(
            ReducedUser reducedUser = new ReducedUser { 
                Id=currentUser.Id,
                Name=currentUser.Name,
                Surname=currentUser.Surname,
                CreatedDate=currentUser.CreatedDate,
                LastIp=currentUser.LastIp,
                LastLogin=currentUser.LastLogin,
                Level=currentUser.Level,
            };

            return reducedUser;
        }
    
        internal UserSchedule GetUserScheduleById(int Id)
        {
            if (Id < 0 && Id != 0)
                return null;

            User user;
            using (var db=new UserContext())
            {
                user=db.Users.FirstOrDefault(u=>u.Id== Id);
            }

            if(user==null)
            {
                return null;
            }

            UserSchedule schedule = new UserSchedule();
            using(var db=new UserContext())
            {
                /*var classes = db.Classes
                    .Include(c=>c.LeadingUser)
                    .Include(c => c.Discipline)  
                    .Include(c => c.Type)       
                    .Include(c => c.Group)        
                    .Include(c => c.Classroom)   
                    .Include(c => c.WeekDay)     
                    .Where(c=>c.LeadingUser.Id==user.Id)
                    .ToList();*/

                var classes = db.Classes
                    .Include(c=>c.UserDiscipline)
                    .Include(c=>c.UserDiscipline.Type)
                    .Include(c=>c.AcademicGroup)
                    .Include(c=>c.ClassRoom)
                    .Include(c=>c.WeekDay)
                    .Include(c=>c.UserDiscipline.Discipline)
                    .Include(c=>c.UserDiscipline.User)
                    .Where(c => c.UserDiscipline.UserId == user.Id)
                    .ToList();
                
                foreach(var lesson in classes)
                {
                    schedule.AddLesson(
                        new Lesson(
                            lesson.UserDiscipline.Discipline.Name,
                            lesson.UserDiscipline.Discipline.ShortName,
                            lesson.UserDiscipline.Type.TypeName,
                            lesson.WeekDay.ShortName,
                            lesson.ClassRoom.ClassroomName,
                            lesson.AcademicGroup.Name,
                            lesson.StartTime,
                            lesson.EndTime,
                            lesson.Frequency
                        )
                    ) ;
                }
            }

            return schedule;
        }

        internal ActionResponse AddNewClassToDb(Class newClass)
        {
            if (newClass == null)
            {
                return new ActionResponse()
                {
                    Status = false,
                    ActionStatusMsg = "Eroare !"
                };
            }
            //to check before insertion !!
            // free classroom +
            // free group at given time (do not forget span) + 
           
            using(var db = new UserContext())
            {
                //check group busy overlap
                //System.Diagnostics.Debug.WriteLine($"grpId: {newClass.AcademicGroup.Id}");

                //check overlap with own schedule, if busy as a human.
                var overlapWithSelf = db.Classes
                    .Include(c => c.UserDiscipline)
                    .Include(c => c.WeekDay)
                    .Include(c => c.UserDiscipline.User)
                    .FirstOrDefault(c => 
                        c.UserDiscipline.User.Id==newClass.UserDiscipline.User.Id &&
                        c.WeekDay.Id == newClass.WeekDay.Id && 
                        (( c.StartTime == newClass.StartTime) || (c.EndTime == newClass.EndTime))
                    );

                //+ duplicates 
                if (overlapWithSelf != null) 
                {
                    return new ActionResponse()
                    {
                        ActionStatusMsg = $"Deja există perechi în acest interval: {newClass.StartTime:hh\\:mm}-{newClass.EndTime:hh\\:mm}",
                        OverlapClassId = overlapWithSelf.Id,
                        Status = false
                    };
                }

                var overlapBusyGroup = db.Classes
                    .FirstOrDefault(
                        c => c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId == newClass.WeekDay.Id &&
                        ((c.StartTime == newClass.StartTime) || (c.EndTime == newClass.EndTime))
                    );
                 
                if (overlapBusyGroup != null) //TEST////TEST////TEST////TEST////TEST////TEST////TEST////TEST//
                {
                    //get group name
                    var overlapGroup=db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                    return new ActionResponse() {
                        ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {newClass.StartTime:hh\\:mm}-{newClass.EndTime:hh\\:mm}",
                        OverlapClassId = overlapBusyGroup.Id,
                        Status = false
                    };
                }

                
                //check mid time span for long classes (add to cabinete Too)
                if(newClass.EndTime - newClass.StartTime != new TimeSpan(1, 30, 0))
                {

                    TimeSpan midTimeSpan;
                    //check pausa del masa
                    if (newClass.StartTime == new TimeSpan(11, 30, 0))
                    {
                        midTimeSpan = newClass.EndTime - new TimeSpan(2, 0, 0);
                    }
                    else
                    {
                        midTimeSpan= newClass.EndTime - new TimeSpan(1, 45, 0); //30 + 15 min accounts for pauză
                    }

                    var startsMidtime = db.Classes.FirstOrDefault(c =>
                        c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId==newClass.WeekDay.Id &&
                        c.StartTime==midTimeSpan
                    );

                    var endsMidtime = db.Classes.FirstOrDefault(c =>
                        c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId == newClass.WeekDay.Id &&
                        c.EndTime == midTimeSpan
                    );

                    if (startsMidtime != null)//test it 
                    {
                        var overlapGroup = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                        return new ActionResponse()
                        {
                            ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {midTimeSpan:hh\\:mm}-{newClass.EndTime:hh\\:mm}",
                            OverlapClassId = startsMidtime.Id,
                            Status = false
                        };
                    }

                    if(endsMidtime != null)
                    {
                        var overlapGroup = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                        return new ActionResponse()
                        {
                            ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {newClass.StartTime:hh\\:mm}-{midTimeSpan:hh\\:mm}",
                            OverlapClassId = endsMidtime.Id,
                            Status = false
                        };
                    }
                }
            }

            //insert stage
            using (var db=new UserContext())
            {
                //get tables UserDiscipline, AcademicGroup, ClassRoom, Weekday, types
                var userDiscipline = db.UserDisciplines
                    .Include(ud => ud.User)
                    .Include(ud => ud.Discipline)
                    .Include(ud => ud.Type)
                    .FirstOrDefault(ud => /*ud.ClassTypeId == newClass.UserDiscipline.ClassTypeId &&*/ ud.DisciplineId == newClass.UserDiscipline.Id);//does not work

                var classType = db.ClassTypes
                    .FirstOrDefault(ud => ud.Id == newClass.UserDiscipline.Type.Id);

                var academicGroup = db.AcademicGroups
                    .FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);

                var classRoom = db.ClassRooms
                    .FirstOrDefault(cr => cr.Id == newClass.ClassRoom.Id);

                var weekDay = db.WeekDays
                    .FirstOrDefault(wd => wd.Id == newClass.WeekDay.Id);
                userDiscipline.Type = classType;
      
                if (userDiscipline != null && academicGroup != null && classRoom != null && weekDay != null)
                {
                    var veryNewClass = new Class
                    {
                        UserDiscipline = userDiscipline,                   
                        AcademicGroup = academicGroup,                      
                        ClassRoom = classRoom,
                        WeekDay = weekDay,
                        StartTime= newClass.StartTime,
                        EndTime = newClass.EndTime,
                        Frequency=newClass.Frequency

                    };
                    db.Classes.Add(veryNewClass);
                    db.SaveChanges();
                }
                else
                {
                    return new ActionResponse { Status = false, ActionStatusMsg = "Internal server error." };
                }
            }

            return new ActionResponse { Status = true };
        }

        internal List<AcademicGroup> GetAcademicGroupsList()
        {
            List<AcademicGroup> groupList;
            using(var db = new UserContext())
            {
                groupList = db.AcademicGroups.ToList();
            }
            if (groupList == null)
                return null;

            return groupList;
        }
        
        internal List<AcademicGroup> GetAcademicGroupsList(int year)
        {
            List<AcademicGroup> groupList;
            using(var db = new UserContext())
            {
                groupList = db.AcademicGroups
                    .Where(a=>a.Year == year )
                    .ToList();
            }
            if (groupList == null)
                return null;

            return groupList;
        }

        internal List<UserDiscipline> GetUserDisciplinesById(int id)
        {
            List<UserDiscipline> discList; //= new List<UserDiscipline>();
            using (var db = new UserContext())
            {
                discList = db.UserDisciplines
                    .Include(ud => ud.Discipline)
                    .Include(ud => ud.User)
                    .Include(ud => ud.Type)
                    .Where(ud => ud.UserId == id).ToList();
            }
            if (discList == null) 
                return new List<UserDiscipline>();
           
            return discList;
        }

        internal List<ClassType> GetUserDisciplineTypesByUserId(int disciplineId, int userId)
        {
            var types = new List<ClassType>();
            using(var db = new UserContext())
            {
                types = db.UserDisciplines
                    .Where(ud => ud.UserId == userId && ud.DisciplineId==disciplineId)
                    .Select(ud => ud.Type)
                    .ToList();
            }
            return types;
        }

        //UNFINISHED////UNFINISHED////UNFINISHED////UNFINISHED////UNFINISHED////UNFINISHED//
        //DO PAR IMPAR FULL CHECKS
        internal List<ClassRoom> GetClassroomsFreeAtTime(FreeClassroomsRequest data)
        {
            //determine end time by startime + 01:30*span $$ exception for pausa del masa span 2.
            TimeSpan endTime;
            if (data.Span == 2)
            {
                if(data.StartTime==new TimeSpan(11, 30, 0))
                    endTime = data.StartTime + new TimeSpan(3, 30, 0);
                else
                    endTime = data.StartTime + new TimeSpan(3, 15, 0);
            }
            else
            {
                endTime=data.StartTime + new TimeSpan(1, 30, 0);
            }

            List<ClassRoom> freeClassrooms=new List<ClassRoom>();

            using (var db = new UserContext())
            {
                ///MISSING-FREQUENCY//////MISSING-FREQUENCY//////MISSING-FREQUENCY//////MISSING-FREQUENCY//////MISSING-FREQUENCY///

                //scot toate cabinetele de pe etaj
                var classRoomsonFloor = db.ClassRooms.Where(cr => cr.Floor == data.Floor).ToList();

                //caut cabinetele ocupate la ora dată //fixit
                var busyClassrooms = db.Classes.Where(cl=>
                    cl.WeekDay.Id == data.WeekdayId &&
                    ((cl.StartTime == data.StartTime) || (cl.EndTime == endTime))
                ).Select(cl=>cl.ClassRoom).ToList();

                //check span two for overlap (midSpan end start)
                if (data.Span == 2)
                {
                    TimeSpan midTimeSpan;
                    //check pausa del masa
                    if (data.StartTime == new TimeSpan(11, 30, 0))
                    {
                        midTimeSpan = endTime - new TimeSpan(2, 0, 0);  //30 + 30 min accounts for pauză de masa
                    }
                    else
                    {
                        midTimeSpan = endTime - new TimeSpan(1, 45, 0); //30 + 15 min accounts for pauză
                    }

                    var ClassBusyMidtimeStart = db.Classes.Where(cl =>
                            cl.WeekDay.Id == data.WeekdayId &&
                            cl.StartTime == midTimeSpan
                        ).Select(cl => cl.ClassRoom).ToList();

                    var ClassBusyMidtimeEnd = db.Classes.Where(cl =>
                            cl.WeekDay.Id == data.WeekdayId &&
                            cl.EndTime == midTimeSpan
                        ).Select(cl => cl.ClassRoom).ToList();

                    
                    busyClassrooms.AddRange(ClassBusyMidtimeStart);
                    busyClassrooms.AddRange(ClassBusyMidtimeEnd);
                }

                //scad din toate cele ocupate 
                freeClassrooms = classRoomsonFloor.Where(x => !busyClassrooms.Contains(x)).ToList();
            }

            return freeClassrooms;
        }
    }
}
