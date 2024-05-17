using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic
{
    public class UserDisciplineBLL : UserDisciplineApi , IUserDiscipline
    {
        public List<UserDiscipline> GetAllUserDiscipline()
        {
            return GetAllUserDisciplineFromDb();
        }

        public ActionResponse DeleteUserDisciplineById(int id)
        {
            return DeleteUserDisciplineByIdFromDb(id);
        }

        public ActionResponse AddUserDiscipline(UserDiscipline newUserDiscipline)
        {
            return AddUserDisciplineToDb(newUserDiscipline);
        }

    }
}
