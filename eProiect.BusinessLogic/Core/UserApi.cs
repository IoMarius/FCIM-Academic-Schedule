using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.Core
{
     public class UserApi
     {
          public ULoginResp RLoginUpService(ULoginData data)
          {
               //Step 1 - SELECT FROM DB>Users WHERE data.
               // PASSWORD == data.Password

               //Step 2 - IF object != NULL
               // CREATE SESSION

               //RETURN SESSION AND STATUS TRUE

               return new ULoginResp { Status = false };
          }
     }
}
