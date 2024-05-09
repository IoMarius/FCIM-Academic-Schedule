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
using NLog.Filters;
using System;

using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
;

using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;


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
                         try
                         {
                               db.UserCredentials.Add(newCredentials);
                               db.Users.Add(newUser);
                               db.SaveChanges();

                         }
                         catch(DbUpdateException ex)
                 {
                              // Handle DbUpdateException
                              System.Diagnostics.Debug.WriteLine("DbUpdateException occurred: " + ex.Message);

                              // Check for inner exceptions
                              if (ex.InnerException != null)
                              {
                                   System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);

                                   // Check if inner exception is SqlException
                                   if (ex.InnerException is SqlException sqlEx)
                                   {
                                        // Handle SqlException
                                        if (sqlEx.Number == 2601) // SQL Server error number for unique constraint violation
                                        {
                                             System.Diagnostics.Debug.WriteLine("Duplicate key error occurred: " + sqlEx.Message);
                                        }
                                        else
                                        {
                                             System.Diagnostics.Debug.WriteLine("Other SQL Server error occurred: " + sqlEx.Message);
                                        }
                                   }
                              }
                         }
                 catch (Exception ex)
                 {
                              // Handle other exceptions
                              System.Diagnostics.Debug.WriteLine("An error occurred: " + ex.Message);
                         }
                    }
     */
               /*  string fromEmail = "vasileb221@gmail.com";
                 MailMessage mailMessage = new MailMessage(fromEmail, "Bercovasile@gmail.com", "Subject", "Body");
                 SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                 smtpClient.UseDefaultCredentials = false;
                 smtpClient.Credentials = new NetworkCredential(fromEmail, "cascaval2024");
                 try
                 { 
                      smtpClient.Send(mailMessage);
                 }
                 catch (Exception ex)
                 {
                      //Error
                      //Console.WriteLine(ex.Message);
                      System.Diagnostics.Debug.WriteLine(ex.Message);
                 }*/

               //TEST
              /* var admin = new AdminApi();

               var newUser = new NewUserData
               {
                    Name = "Ion",
                    Surname = "Ionescu",
                    Email = "bercovasile@gmail.com",
                    Level = UserRole.admin
               };
               var acction = admin.AddNewUser(newUser);*/
               /*DeleteUser(1);*/

               /*  var newDiscipline = new Discipline
                 {
                      Name = "Matematic speciale 1",
                      ShortName = "MS1"
                 };
                 var action = AddNewDiscipline(newDiscipline);

                 var newDiscipline1 = new Discipline
                 {
                      Name = "Matematic speciale 2",
                      ShortName = "MS2"
                 };
                 var action1 = AddNewDiscipline(newDiscipline1);*/

               User result;
               var validate = new EmailAddressAttribute();
               if (validate.IsValid(data.Credential))
               {
                    var pass = LoginHelper.HashGen(data.Password);

                    using (var db = new UserContext())
                    {
                         result = db.Users.FirstOrDefault(u => u.Credentials.Email == data.Credential && u.Credentials.Password == pass);
                    }

                    if (result == null)
                    {
                         System.Diagnostics.Debug.WriteLine("ULoginResp returned status {FALSE}. Incorrect password or username.");
                         return new ActionResponse { Status = false, ActionStatusMsg = "The username or password is incorrect." };
                    }

                    using (var todo = new UserContext())
                    {
                         result.LastIp = data.LoginIp;
                         result.LastLogin = data.LoginDateTime;
                         todo.Entry(result).State = EntityState.Modified;
                         todo.SaveChanges();
                    }
                    System.Diagnostics.Debug.WriteLine("ULoginResp returned status {TRUE}.");
                    return new ActionResponse { Status = true };
               }
               else
               {
                    //logging with something other than email, like username
                    //but there are no usernames here I think..
                    System.Diagnostics.Debug.WriteLine("ULoginResp returned status {FALSE}. Invalid email address.");
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

          internal ReducedUser UserByCookie(string cookieString)
          {
               Session session;
               User currentUser;

               using (var db = new SessionContext())
               {
                    //checking if the cookie exists in the database and it has not expired yet.
                    session = db.Sessions.FirstOrDefault(s => s.CookieString == cookieString && s.ExpireTime > DateTime.Now);
               }

               if (session == null)
                    return null;

               using (var db = new UserContext())
               {
                    //query existing users and identify to which the email belongs from the cookies table.
                    currentUser = db.Users.FirstOrDefault(u => u.Credentials.Email == session.Email);
               }

               if (currentUser == null)
                    return null;

               //use mapper, but for now no mapper :(
               ReducedUser reducedUser = new ReducedUser
               {
                    Id = currentUser.Id,
                    Name = currentUser.Name,
                    Surname = currentUser.Surname,
                    CreatedDate = currentUser.CreatedDate,
                    LastIp = currentUser.LastIp,
                    LastLogin = currentUser.LastLogin,
                    Level = currentUser.Level,
               };

               return reducedUser;
          }

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
                        .Include(c => c.UserDiscipline)
                        .Include(c => c.UserDiscipline)
                        .Include(c => c.ClassRoom)
                        .Include(c => c.WeekDay)
                        .Include(c => c.UserDiscipline.Discipline)
                        .Include(c => c.UserDiscipline.User)
                        .Where(c => c.UserDiscipline.UserId == user.Id);

                    foreach (var lesson in classes)
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
                         );
                    }
               }

               return schedule;
          }

         /* internal GroupOfUsers GetAllUser()
          {
               var allUsers = new GroupOfUsers();

               using (var db = new UserContext())
               {
                    allUsers.Users = db.Users.Include(u => u.Credentials).ToList();
               }
               return allUsers;
          }
          internal ActionResponse AddNewUser(NewUserData newUserData)
          {
               var validate = new EmailAddressAttribute();
               if (!validate.IsValid(newUserData.Email))
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Is'n valid Email",
                         Status = false
                    };
               }


               var _password = Membership.GeneratePassword(12, 0);
               var  mesages = "This is your password " + _password;
               if (!SendEmail.SendEmailToUser(newUserData.Email,newUserData.Name, "Password for Shedules Platform" , mesages))
                    return new ActionResponse
                    {
                         ActionStatusMsg = "The messaging service is temporarily not working",
                         Status = false
                    };

               var newCredentials = new UserCredential
               {
                    Email = newUserData.Email,
                    Password = LoginHelper.HashGen(_password),
               };

               var newUser = new User
               {
                    Name = newUserData.Name,
                    Surname = newUserData.Surname,
                    Level = newUserData.Level,
                    CreatedDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    Credentials = newCredentials
               };

               using (var db = new UserContext())
               {

                    try
                    {
                         db.UserCredentials.Add(newCredentials);
                         db.Users.Add(newUser);
                         db.SaveChanges();

                    }
                    catch (DbUpdateException ex)
                    {
                         if (ex.InnerException != null)
                         {

                              // Check if inner exception is SqlException
                              if (ex.InnerException is SqlException sqlEx)
                              {
                                   // Handle SqlException
                                   if (sqlEx.Number == 2601) // SQL Server error number for unique constraint violation
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Duplicate key error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                                   else
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Other SQL Server error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                              }
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Inner Exception: " + ex.InnerException.Message,
                                   Status = false
                              };
                         }
                         return new ActionResponse
                         {
                              ActionStatusMsg = "DbUpdateException occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    catch (Exception ex)
                    {
                         // Handle other exceptions
                         return new ActionResponse
                         {
                              ActionStatusMsg = "An error occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    return new ActionResponse { Status = true };
               }

          }
          internal ActionResponse DeleteUser(int Id)
          {
               if (Id < 0 && Id != 0)
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Invalid ID",
                         Status = false
                    };
               try
               {
                    UserCredential _user;
                    using (var db = new UserContext())
                    {
                         _user = db.UserCredentials.FirstOrDefault(g => g.Id == Id);
                         if (_user == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "User not found or invalid ID",
                                   Status = false
                              };
                         }


                         db.UserCredentials.Remove(_user);

                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating user data: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "User data updated successfully",
                    Status = true
               };
          }
          internal ActionResponse EditUser(User newUserData)
          {
               if (newUserData == null)
               {

                    return new ActionResponse
                    {
                         ActionStatusMsg = "Null parametre user",
                         Status = false
                    };
               }
               var validate = new EmailAddressAttribute();
               if (!validate.IsValid(newUserData.Credentials.Email))

               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Is'n valid Email",
                         Status = false
                    };
               }
               try
               {
                    using (var db = new UserContext())
                    {
                         var _user = db.Users.Include(u => u.Credentials).FirstOrDefault(u => u.Id == newUserData.Id);
                         if (_user == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "User not found or invalid ID",
                                   Status = false
                              };
                         }

                         _user.Name = newUserData.Name;
                         _user.Surname = newUserData.Surname;
                         _user.Credentials.Email = newUserData.Credentials.Email;
                         _user.Level = newUserData.Level;
                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating user data: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "User data updated successfully",
                    Status = true
               };



          }
*/

          /*internal GroupOfDisciplines GetAllDiscipline()
          {
               var allDisciplines = new GroupOfDisciplines();

               using (var db = new UserContext())
               {
                    allDisciplines.Disciplines = db.Disciplines.ToList();
               }
               return allDisciplines;
          }
          internal ActionResponse AddNewDiscipline(Discipline discipline)
          {
               var newDiscipline = new Discipline
               {
                    Name = discipline.Name,
                    ShortName = discipline.ShortName
               };

               using (var db = new UserContext())
               {

                    try
                    {
                         db.Disciplines.Add(newDiscipline);
                         db.SaveChanges();

                    }
                    catch (DbUpdateException ex)
                    {
                         if (ex.InnerException != null)
                         {

                              // Check if inner exception is SqlException
                              if (ex.InnerException is SqlException sqlEx)
                              {
                                   // Handle SqlException
                                   if (sqlEx.Number == 2601) // SQL Server error number for unique constraint violation
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Duplicate key error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                                   else
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Other SQL Server error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                              }
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Inner Exception: " + ex.InnerException.Message,
                                   Status = false
                              };
                         }
                         return new ActionResponse
                         {
                              ActionStatusMsg = "DbUpdateException occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    catch (Exception ex)
                    {
                         // Handle other exceptions
                         return new ActionResponse
                         {
                              ActionStatusMsg = "An error occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    return new ActionResponse { Status = true };
               }
          }
          internal ActionResponse EditDiscipline(Discipline updatedDisciplineData)
          {
               try
               {
                    using (var db = new UserContext())
                    {
                         var discipline = db.Disciplines.FirstOrDefault(d => d.Id == updatedDisciplineData.Id);
                         if (discipline == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Discipline not found or invalid ID",
                                   Status = false
                              };
                         }

                         // Update discipline properties
                         discipline.Name = updatedDisciplineData.Name;
                         discipline.ShortName = updatedDisciplineData.ShortName;

                         db.SaveChanges();
                    }

                    return new ActionResponse
                    {
                         ActionStatusMsg = "Discipline data updated successfully",
                         Status = true
                    };
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating discipline data: {ex.Message}",
                         Status = false
                    };
               }
          }
          internal ActionResponse DeleteDiscipline(int Id)
          {
               if (Id < 0 && Id != 0)
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Invalid ID",
                         Status = false
                    };
               try
               {
                    Discipline _discipline;
                    using (var db = new UserContext())
                    {
                         _discipline = db.Disciplines.FirstOrDefault(d => d.Id == Id);
                         if (_discipline == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Discipline not found or invalid ID",
                                   Status = false
                              };
                         }

                         db.Disciplines.Remove(_discipline);
                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating discipline data: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "Discipline data updated successfully",
                    Status = true
               };

          }*/
          
          
          /*internal AcademicGroupsList GetAllAcademicGroup()
          {
               var allAcademicGroups = new AcademicGroupsList();

               using (var db = new UserContext())
               {
                    allAcademicGroups.AcademicGroups = db.AcademicGroups.ToList();
               }
               return allAcademicGroups;
          }
          internal ActionResponse AddNewAcademicGroup(AcademicGroup academicGroup)
          {
               var newAcademicGroup = new AcademicGroup
               {
                    Name = academicGroup.Name,
                    Year = academicGroup.Year
               };

               using (var db = new UserContext())
               {

                    try
                    {
                         db.AcademicGroups.Add(newAcademicGroup);
                         db.SaveChanges();

                    }
                    catch (DbUpdateException ex)
                    {
                         if (ex.InnerException != null)
                         {

                              // Check if inner exception is SqlException
                              if (ex.InnerException is SqlException sqlEx)
                              {
                                   // Handle SqlException
                                   if (sqlEx.Number == 2601) // SQL Server error number for unique constraint violation
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Duplicate key error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                                   else
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Other SQL Server error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                              }
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Inner Exception: " + ex.InnerException.Message,
                                   Status = false
                              };
                         }
                         return new ActionResponse
                         {
                              ActionStatusMsg = "DbUpdateException occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    catch (Exception ex)
                    {
                         // Handle other exceptions
                         return new ActionResponse
                         {
                              ActionStatusMsg = "An error occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    return new ActionResponse { Status = true };
               }
          }
          internal ActionResponse EditAcademicGroup(AcademicGroup newAcademicGroupData)
          {
               if (newAcademicGroupData == null)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Null parametre newAcademicGroupData",
                         Status = false
                    };
               }
               var validate = new EmailAddressAttribute();

               try
               {
                    using (var db = new UserContext())
                    {
                         var _academicGroup = db.AcademicGroups.FirstOrDefault(u => u.Id == newAcademicGroupData.Id);
                         if (_academicGroup == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Academic group not found or invalid ID",
                                   Status = false
                              };
                         }

                         _academicGroup.Name = newAcademicGroupData.Name;
                         _academicGroup.Year = newAcademicGroupData.Year;

                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating user data: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "User data updated successfully",
                    Status = true
               };


          }
          internal ActionResponse DeleteAcademicGroup(int Id)
          {
               if (Id < 0 && Id != 0)
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Invalid ID",
                         Status = false
                    };
               try
               {
                    AcademicGroup _academicGroup;
                    using (var db = new UserContext())
                    {
                         _academicGroup = db.AcademicGroups.FirstOrDefault(g => g.Id == Id);
                         if (_academicGroup == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Academic group not found or invalid ID",
                                   Status = false
                              };
                         }

                         db.AcademicGroups.Remove(_academicGroup);
                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating academic group data: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "Academic group data updated successfully",
                    Status = true
               };
          }*/


          /*internal GroupOfCassRooms GetAllClassRoom()
          {
               var allCassRooms = new GroupOfCassRooms();

               using (var db = new UserContext())
               {
                    allCassRooms.CassRooms = db.ClassRooms.ToList();
               }
               return allCassRooms;
          }
          internal ActionResponse AddNewClassRoom(ClassRoom classRoom)
          {
               var newClassRoom = new ClassRoom
               {
                    ClassroomName = classRoom.ClassroomName,
                    Floor = classRoom.Floor
               };

               using (var db = new UserContext())
               {

                    try
                    {
                         db.ClassRooms.Add(newClassRoom);
                         db.SaveChanges();

                    }
                    catch (DbUpdateException ex)
                    {
                         if (ex.InnerException != null)
                         {

                              // Check if inner exception is SqlException
                              if (ex.InnerException is SqlException sqlEx)
                              {
                                   // Handle SqlException
                                   if (sqlEx.Number == 2601) // SQL Server error number for unique constraint violation
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Duplicate key error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                                   else
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Other SQL Server error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                              }
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Inner Exception: " + ex.InnerException.Message,
                                   Status = false
                              };
                         }
                         return new ActionResponse
                         {
                              ActionStatusMsg = "DbUpdateException occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    catch (Exception ex)
                    {
                         // Handle other exceptions
                         return new ActionResponse
                         {
                              ActionStatusMsg = "An error occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    return new ActionResponse { Status = true };
               }
          }
          internal ActionResponse EditClassRoom(ClassRoom updatedClassRoomData)
          {
               try
               {
                    using (var db = new UserContext())
                    {
                         var classRoom = db.ClassRooms.FirstOrDefault(c => c.Id == updatedClassRoomData.Id);
                         if (classRoom == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Classroom not found or invalid ID",
                                   Status = false
                              };
                         }

                         // Update classroom properties
                         classRoom.ClassroomName = updatedClassRoomData.ClassroomName;
                         classRoom.Floor = updatedClassRoomData.Floor;

                         db.SaveChanges();
                    }

                    return new ActionResponse
                    {
                         ActionStatusMsg = "Classroom data updated successfully",
                         Status = true
                    };
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating classroom data: {ex.Message}",
                         Status = false
                    };
               }
          }
          internal ActionResponse DeleteClassRoom(int Id)
          {
               if (Id < 0 && Id != 0)
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Invalid ID",
                         Status = false
                    };
               try
               {
                    ClassRoom _classRoom;
                    using (var db = new UserContext())
                    {
                         _classRoom = db.ClassRooms.FirstOrDefault(c => c.Id == Id);
                         if (_classRoom == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Nu a fost gasita ClassRoom sau este id invalid",
                                   Status = false
                              };
                         }

                         db.ClassRooms.Remove(_classRoom);
                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"A aparut o eroare la stergerea ClassRoom: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "ClassRoom a fost stearsa cu succes",
                    Status = true
               };
          }*/

     }

}
