using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User.DBModel;
using eProiect.Domain.Entities.User;
using eProiect.Helper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity;
using System.Web.Security;
using System.Collections.Generic;
using NLog.LayoutRenderers.Wrappers;


namespace eProiect.BusinessLogic.Core
{
     public class AdminApi
     {
          internal GroupOfUsers GetAllUser()
          {
               var allUsers = new GroupOfUsers();

               using (var db = new UserContext())
               {
                    allUsers.Users = db.Users.Include(u => u.Credentials).ToList();
               }
               return allUsers;
          }
          public ActionResponse AddNewUser(NewUserData newUserData)
          {
               var validate = new EmailAddressAttribute();
               if (!validate.IsValid(newUserData.Email))
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Isn't valid Email",
                         Status = false
                    };
               }



               var _password = Membership.GeneratePassword(12, 0);
               var mesages = $"Salut {newUserData.Name} {newUserData.Surname},\n Parola de acces la platforma orar FCIM: {_password}";

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
                    Birthday = new DateTime(1900, 1, 1),
                    CreatedDate = DateTime.Now,
                    LastLogin = new DateTime(2000, 1, 1),
                    Credentials = newCredentials
               };
               if (!SendEmail.SendEmailToUser(newUserData.Email, newUserData.Name, "Password for Shedules Platform", mesages)) 
                    return new ActionResponse
                    {
                         ActionStatusMsg = "The messaging service is temporarily not working or email is wrong!",
                         Status = false
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
                    return new ActionResponse 
                    {
                         ActionStatusMsg = "User succesed added ",
                         Status = true 
                    };

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
                    User _user;
                    using (var db = new UserContext())
                    {
                         _user = db.Users.FirstOrDefault(g => g.Id == Id);
                         var _userCredentials = db.UserCredentials.FirstOrDefault(c => c.Id == _user.UserCredentialId);
                         if (_user == null || _userCredentials == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "User not found or invalid ID",
                                   Status = false
                              };
                         }
                         db.UserCredentials.Remove(_userCredentials);

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
               if (newUserData.Birthday == new DateTime(0001, 1, 1)) 
               {
                    newUserData.Birthday = new DateTime(1900, 1, 1);
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
                         _user.Birthday = newUserData.Birthday;
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

        internal List<User> GetUsersRedusedUserData()
        {
            var user = new List<User>();

            using (var db = new UserContext())
            {
                var userReduses = db.Users
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Surname = c.Surname
                    })
                    .ToList();
                user = userReduses.Select(c => new User
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname
                })
                    .ToList();
            }
            return user;
        }

        

    }
}
