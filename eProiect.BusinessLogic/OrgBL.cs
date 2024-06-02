using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.Schedule;
using eProiect.Domain.Entities.User.DBModel;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.Domain.Entities.Academic;

namespace eProiect.BusinessLogic
{
    public class OrgBL : OrgApi, IOrg
    {
        
        public UserSchedule GetScheduleById(int id)
        {
            return GetUserScheduleById(id);
        }

        public List<Discipline> GetDisciplinesById(int id)
        {
            return GetUserDisciplinesById(id);
        }

        public List<ClassType> GetTypesByDisciplineForUser(int disciplineId, int userId)
        {
            return GetUserDisciplineTypesByUserId(disciplineId, userId);
        }

        public bool IsCurrentWeekEven()
        {
            return IsEvenWeek();
        }

        public List<User> GetTeacherUsers()
        {
            return GetAllTeacherUsers();
        }

    }
}
