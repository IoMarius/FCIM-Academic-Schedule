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
     public class AdminBL : AdminApi , IAdmin
     {
          public GroupOfUsers GetAllUsers()
          {
               return GetAllUser();
          }
          public ActionResponse AddNewUsers(NewUserData newUserData)
          {
               return AddNewUser(newUserData);
          }
          public ActionResponse EditUsers(User editedUserData)
          {
               return EditUser(editedUserData);
          }
          public ActionResponse DeleteUsers(int Id)
          {
               return DeleteUser(Id);
          }
        public List<User> GetAllUserRedusedUserData()
        {
            return GetUsersRedusedUserData();
        }
    }
}
