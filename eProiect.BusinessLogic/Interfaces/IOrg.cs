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

namespace eProiect.BusinessLogic.Interfaces
{
    /// <summary>
    /// Organizational methods for getting info about current classes
    /// and using it to insert new classes
    /// </summary>
    public interface IOrg 
    {
        UserSchedule GetScheduleById(int id);

        List<AcademicGroup> GetAcadGroupsList();

        List<AcademicGroup> GetAcadGroupsList(int year);

        List<Discipline> GetDisciplinesById(int id);

        List<ClassRoom> GetFreeClassroomsByFloorAndTime(FreeClassroomsRequest data);

        List<ClassType> GetTypesByDisciplineForUser(int disciplineId, int userId);

        ActionResponse AddNewClass(Class newClass);
    }
}
