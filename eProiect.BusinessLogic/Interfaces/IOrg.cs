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

namespace eProiect.BusinessLogic.Interfaces
{
    /// <summary>
    /// Organizational methods for getting info about current classes
    /// and using it to insert new classes
    /// </summary>
    public interface IOrg 
    {
        /// <summary>
        /// Queries the database for all classes linked to the 
        /// user with the provided ID, returning an instance of
        /// "UserSchedule" class.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserSchedule</returns>
        UserSchedule GetScheduleById(int id);

        /// <summary>
        /// Queries the database table "UserDisciplines" for the disciplines
        /// linked to the user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of instances of "Discipline" class</returns>
        List<Discipline> GetDisciplinesById(int id);

        /// <summary>
        /// Queries the database table "UserDisciplines" for "Types" of disciplines
        /// of an user.
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <param name="userId"></param>
        /// <returns>List of instances of "ClassType"</returns>
        List<ClassType> GetTypesByDisciplineForUser(int disciplineId, int userId);

        /// <summary>
        /// Returns all users from database with level==1
        /// teacher level. Excluding sensitive information.
        /// </summary>
        /// <returns></returns>
        List<User> GetTeacherUsers();

        /// <summary>
        /// Used for even odd lessons in schedule
        /// </summary>
        /// <returns>Self explainatory</returns>
        bool IsCurrentWeekEven();

        /// <summary>
        /// Subscribe an user to email notifications when the desired academic group 
        /// schedule is being modified
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ActionResponse SubscribeUserToNewsletter(SubscribeUserRequest data);
    }
}
