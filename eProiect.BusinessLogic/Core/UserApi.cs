using AutoMapper;
using eProiect.BusinessLogic.DBContext;
using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;
using eProiect.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eProiect.BusinessLogic.Core
{
     public class UserApi
     {
        internal ULoginResp RLoginUpService(ULoginData data)
        {
            //TEST ADDING USR
            using (var db = new UserContext())
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
            }

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
                    System.Diagnostics.Debug.WriteLine("ULoginResp returned status {FALSE}. Incorrect password or username.");
                    return new ULoginResp { Status = false, ActionStatusMsg = "The username or password is incorrect." };
                }

                using (var todo=new UserContext())
                {
                    result.LastIp = data.LoginIp;
                    result.LastLogin = data.LoginDateTime;
                    todo.Entry(result).State= EntityState.Modified;
                    todo.SaveChanges();
                }
                System.Diagnostics.Debug.WriteLine("ULoginResp returned status {TRUE}.");
                return new ULoginResp { Status = true };
            }
            else
            {
                //logging with something other than email, like username
                //but there are no usernames here I think..
                System.Diagnostics.Debug.WriteLine("ULoginResp returned status {FALSE}. Invalid email address.");
                return new ULoginResp { Status = false, ActionStatusMsg = "Invalid email address." };
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
                    .Include(c=>c.UserDiscipline)
                    .Include(c=>c.ClassRoom)
                    .Include(c=>c.WeekDay)
                    .Include(c=>c.UserDiscipline.Discipline)
                    .Include(c=>c.UserDiscipline.User)
                    .Where(c => c.UserDiscipline.UserId == user.Id);
                
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
     }
}
