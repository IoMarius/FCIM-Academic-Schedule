using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eProiect.BusinessLogic
{
     internal class SessionAdminBL :AdminApi 
     {
          public ActionResponse UserLoginAction(ULoginData data)
          {
               return RLoginUpService(data);
          }

          public HttpCookie GenCookie(string loginCredential)
          {
               return Cookie(loginCredential);
          }

          public ReducedUser GetUserByCookie(string cookieString)
          {
               return UserByCookie(cookieString);
          }

          public UserSchedule GetScheduleById(int id)
          {
               return GetUserScheduleById(id);
          }

     }
}
