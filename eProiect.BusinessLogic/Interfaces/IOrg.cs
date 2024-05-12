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
        /// <summary>
        /// Queries the database for all classes linked to the 
        /// user with the provided ID, returning an instance of
        /// "UserSchedule" class.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserSchedule</returns>
        UserSchedule GetScheduleById(int id);

        /// <summary>
        /// Queries the database table "AcademicGroups" selecting all
        /// academic groups available.
        /// </summary>
        /// <returns>List of class instance "AcademicGroup".</returns>
        List<AcademicGroup> GetAcadGroupsList();

        /// <summary>
        /// (Overloaded "GetAcadGroupsList()")
        /// Queries the database table "AcademicGroups" selecting all
        /// academic groups of a coresponding year.
        /// </summary>
        /// <param name="year">Academic group year</param>
        /// <returns></returns>
        List<AcademicGroup> GetAcadGroupsList(int year);

        /// <summary>
        /// Queries the database table "UserDisciplines" for the disciplines
        /// linked to the user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of instances of "Discipline" class</returns>
        List<Discipline> GetDisciplinesById(int id);

        /// <summary>
        /// Queries the database table "Classrooms" for free classrooms at
        /// given parameters(weekday, time, span, frequency);
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<ClassRoom> GetFreeClassroomsByFloorAndTime(FreeClassroomsRequest data);

        /// <summary>
        /// Queries the database table "UserDisciplines" for "Types" of disciplines
        /// of an user.
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <param name="userId"></param>
        /// <returns>List of instances of "ClassType"</returns>
        List<ClassType> GetTypesByDisciplineForUser(int disciplineId, int userId);

        /// <summary>
        /// Queries the database table "Classes" for a single class
        /// with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns an instance of "Class"</returns>
        Class GetClassById(int id);

        /// <summary>
        /// Adds a new entry in the table "Classes".
        /// </summary>
        /// <param name="newClass"></param>
        /// <returns>Status of the action.</returns>
        ActionResponse AddNewClass(Class newClass);


        /// <summary>
        /// Edit an existing entry from table "Classses"
        /// where modifiedClasss.Id is met.
        /// </summary>
        /// <param name="modifiedClasss"></param>
        /// <returns>Status of the action.</returnsa>
        ActionResponse EditExistingClass(Class modifiedClasss);

        /// <summary>
        /// Delete records from "Classes" table by Id.
        /// </summary>
        /// <param name="id">Existing "Class" Id</param>
        /// <returns>ActionResponse</returns>
        ActionResponse DeleteClassById(int id);

        /// <summary>
        /// Used for getting all available classes for an academic group
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of Class instances</returns>
        List<Class> GetAcademicGroupClasses(int id);

        /// <summary>
        /// Used for even odd lessons in schedule
        /// </summary>
        /// <returns>Self explainatory</returns>
        bool IsCurrentWeekEven();
    }
}
