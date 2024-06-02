using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.Interfaces
{
     public interface IAcademicGroup
     {
          AcademicGroupsList GetAllAcademicGroups();
          ActionResponse AddNewAcademicGroups(AcademicGroup academicGroup);
          ActionResponse EditAcademicGroups(AcademicGroup updateAcademicGroup);
          ActionResponse DeleteAcademicGroups(int Id);

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
    }
}
