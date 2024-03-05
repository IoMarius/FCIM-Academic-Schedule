using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic
{
     public class SessionBL : UserApi, ISession
     {
          public ULoginResp UserLoginAction(ULoginData data)
          {
               return RLoginUpService(data);
          }

     }
}
