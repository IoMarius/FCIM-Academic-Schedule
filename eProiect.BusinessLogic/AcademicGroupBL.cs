using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;

namespace eProiect.BusinessLogic
{
     public class AcademicGroupBL : AcademicGroupApi , IAcademicGroup
     {
          public AcademicGroupsList GetAllAcademicGroups()
          {
               return GetAllAcademicGroup();
          }
          public ActionResponse AddNewAcademicGroups(AcademicGroup academicGroup)
          {
               return AddNewAcademicGroup(academicGroup);
          }
          public ActionResponse EditAcademicGroups(AcademicGroup updateAcademicGroup)
          {
               return EditAcademicGroup(updateAcademicGroup);
          }
          public ActionResponse DeleteAcademicGroups(int Id)
          {
               return DeleteAcademicGroup(Id);
          }
            public List<AcademicGroup> GetAcadGroupsList()
            {
                return GetAcademicGroupsList();
            }

            public List<AcademicGroup> GetAcadGroupsList(int year)
            {
                return GetAcademicGroupsList(year);
            }
     }
}
