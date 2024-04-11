using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eProiect.BusinessLogic.Interfaces
{
     public interface ISession
     {
          ULoginResp UserLoginAction(ULoginData data);

          HttpCookie GenCookie(string loginCredential);

          ReducedUser GetUserByCookie(string cookieString);

          void TestDb();
    }
}
