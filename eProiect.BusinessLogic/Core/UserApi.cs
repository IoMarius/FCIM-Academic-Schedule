using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;
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
            UserCredential user;
            using (var db = new UserContext())
            {
                user = db.UserCredentials.FirstOrDefault(u => u.Email == data.Credential && u.Password == data.Password);
            }

            if (user != null)
            {
                return new ULoginResp { Status = true };
            }
            else
            {
                return new ULoginResp { Status = false };
            }
        }

    }
}
