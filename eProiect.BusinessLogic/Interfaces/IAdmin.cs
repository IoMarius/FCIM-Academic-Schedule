using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.Interfaces
{
     public interface IAdmin 
     {
          GroupOfUsers GetAllUsers();
          ActionResponse AddNewUsers(NewUserData newUserData);
          ActionResponse EditUsers(User editedUserData);
          ActionResponse DeleteUsers(int Id);
          List<User> GetAllUserRedusedUserData();
     }
}
