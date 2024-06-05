using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.Schedule;
using eProiect.Domain.Entities.User.DBModel;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security;
using System.Net.Security;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;


namespace eProiect.BusinessLogic.Core
{
    public class OrgApi
    {
        internal UserSchedule GetUserScheduleById(int Id)
        {
            if (Id < 0 && Id != 0)
                return null;

            User user;
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Id == Id);
            }

            if (user == null)
            {
                return null;
            }

            UserSchedule schedule = new UserSchedule();
            using (var db = new UserContext())
            {
                var classes = db.Classes
                    .Include(c => c.UserDiscipline)
                    .Include(c => c.UserDiscipline.Type)
                    .Include(c => c.AcademicGroup)
                    .Include(c => c.ClassRoom)
                    .Include(c => c.WeekDay)
                    .Include(c => c.UserDiscipline.Discipline)
                    .Include(c => c.UserDiscipline.User)
                    .Where(c => c.UserDiscipline.UserId == user.Id)
                    .ToList();

                foreach (var lesson in classes)
                {
                    schedule.AddLesson(
                        new Lesson(
                            lesson.Id,
                            lesson.UserDiscipline.Discipline.Name,
                            lesson.UserDiscipline.Discipline.ShortName,
                            lesson.UserDiscipline.Type.TypeName,
                            lesson.WeekDay.ShortName,
                            lesson.ClassRoom.ClassroomName,
                            lesson.AcademicGroup.Name,
                            lesson.StartTime,
                            lesson.EndTime,
                            lesson.IsConfirmed,
                            lesson.Frequency
                        )
                    );
                }
            }

            return schedule;
        }

        internal List<Discipline> GetUserDisciplinesById(int id)
        {
            List<Discipline> discList; //= new List<UserDiscipline>();
            using (var db = new UserContext())
            {
                discList = db.UserDisciplines
                    .Include(ud => ud.Discipline)
                    .Include(ud => ud.User)
                    .Include(ud => ud.Type)
                    .Where(ud => ud.UserId == id)
                    .Select(ud => ud.Discipline)
                    .ToList();
            }
            if (discList == null)
                return new List<Discipline>();

            return discList.Distinct().ToList();
        }

        internal List<ClassType> GetUserDisciplineTypesByUserId(int disciplineId, int userId)
        {
            var types = new List<ClassType>();
            using (var db = new UserContext())
            {
                types = db.UserDisciplines
                    .Where(ud => ud.UserId == userId && ud.DisciplineId == disciplineId)
                    .Select(ud => ud.Type)
                    .ToList();
            }
            return types;
        }

        internal List<User> GetAllTeacherUsers()
        {
            var users = new List<User>();
            using(var db=new UserContext())
            {
                users = db.UserDisciplines
                    .Select(Ud => Ud.User)
                    .Distinct()
                    .ToList();
            }
            return users;
        }

        internal ActionResponse SubscribeUserToNewsletterAction(SubscribeUserRequest data)
        {
            var EmailValid = new EmailAddressAttribute();
            
            if (!EmailValid.IsValid(data.GuestEmail))
            {
                return new ActionResponse() { Status = false, ActionStatusMsg = "Adresă email eronată" };
            }
            if(data.GuestEmail == null)
            {
                return new ActionResponse() { Status = false, ActionStatusMsg = "Introduceți adresa email" };
            }
            if(data.GuestEmail.Length > 90)
            {
                return new ActionResponse() { Status = false, ActionStatusMsg = "Adresa email depășește limita de lungime(90 caractere)." };
            }
            

            try
            {
                using (var db = new UserContext())
                {
                    var subscriber = db.Students.FirstOrDefault(s => s.AcademicGroupId == data.AcademicGroupId && s.Email == data.GuestEmail);

                    if (subscriber != null)
                    {
                        return new ActionResponse() { Status = false, ActionStatusMsg = "Deja sunteți abonat" };
                    }
                    else
                    {
                        var newStudent = new Students
                        {
                            AcademicGroupId = data.AcademicGroupId,
                            Email = data.GuestEmail,
                        };

                        db.Students.Add(newStudent);                                               
                        db.SaveChanges();

                        return new ActionResponse() { Status = true, ActionStatusMsg = "Success" };
                    }
                }            
            }catch(Exception ex)
            {
                System.Diagnostics.Debug
                    .WriteLine($"SubscribeUserToNewsletter(Email:{data.GuestEmail}|GroupId:{data.AcademicGroupId}):{ex.Message}. Details: {ex.InnerException}");
                return new ActionResponse() { Status = false, ActionStatusMsg = "Internal error" };
            }
        }

        internal bool IsEvenWeek()
        {            
            DateTime today = DateTime.Today;
            DayOfWeek currentDayOfWeek = today.DayOfWeek;

            int daysToSubtract = ((int)currentDayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            var mondayDate = today.AddDays(-daysToSubtract);
            
            if (mondayDate.Day % 2 == 0)
                return true;
            return false;                        
        }
    }
}
