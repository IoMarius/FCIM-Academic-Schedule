using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.Interfaces
{
    public interface IUserDiscipline
    {
        List<UserDiscipline> GetAllUserDiscipline();
        ActionResponse DeleteUserDisciplineById(int id);
        ActionResponse AddUserDiscipline(UserDiscipline newUserDiscipline);
    }
}
