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


using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;


namespace eProiect.BusinessLogic.Core
{
     public class UserApi
     {

        internal ActionResponse RLoginUpService(ULoginData data)
        {
           
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
    }

}
