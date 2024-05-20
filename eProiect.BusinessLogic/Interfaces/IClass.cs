using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.Interfaces
{
    public interface IClass
    {
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
        /// Used for getting all classes of a user from classes table
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>List of classes</returns>
        List<Class> GetUserClasses(int id);

        /// <summary>
        /// Get all unconfirmed classes.
        /// </summary>
        /// <returns></returns>
        List<Class> GetPendingConfirmClasses();

        /// <summary>
        /// Groups all classes overlapping classes into a list.
        /// </summary>
        /// <param name="classes">List of all pending classes</param>
        /// <returns>A list of organized lists made from overlapping classes</returns>
        List<OverlapClassGroup> GroupConflictingClasses(List<Class> classes);

        /// <summary>
        /// Used to confirm a class with pending status.
        /// </summary>
        /// <param name="id">Class id in classes db table</param>
        /// <returns>Action response with action status and message.</returns>
        ActionResponse ConfirmPendingClass(int id);
    }
}
