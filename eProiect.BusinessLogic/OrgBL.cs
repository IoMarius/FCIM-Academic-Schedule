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

namespace eProiect.BusinessLogic
{
    public class OrgBL : OrgApi, IOrg
    {
        public UserSchedule GetScheduleById(int id)
        {
            return GetUserScheduleById(id);
        }

        public List<AcademicGroup> GetAcadGroupsList()
        {
            return GetAcademicGroupsList();
        }

        public List<AcademicGroup> GetAcadGroupsList(int year)
        {
            return GetAcademicGroupsList(year);
        }

        public List<UserDiscipline> GetDisciplinesById(int id)
        {
            return GetUserDisciplinesById(id);
        }

        public List<ClassRoom> GetFreeClassroomsByFloorAndTime(FreeClassroomsRequest data)
        {
            return GetClassroomsFreeAtTime(data);
        }

        public ActionResponse AddNewClass(Class newClass)
        {
            return AddNewClassToDb(newClass);
        }

        public List<ClassType> GetTypesByDisciplineForUser(int disciplineId, int userId)
        {
            return GetUserDisciplineTypesByUserId(disciplineId, userId);
        }
    }
}
