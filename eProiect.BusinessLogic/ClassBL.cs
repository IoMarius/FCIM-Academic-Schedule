using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic
{
    public class ClassBL: ClassApi, IClass
    {
        public ActionResponse AddNewClass(Class newClass)
        {
            return AddNewClassToDb(newClass);
        }

        public Class GetClassById(int id)
        {
            return GetClass(id);
        }

        public ActionResponse EditExistingClass(Class modifiedClasss)
        {
            return EditClass(modifiedClasss);
        }

        public ActionResponse DeleteClassById(int id)
        {
            return RemoveUserClassById(id);
        }

        public List<Class> GetAcademicGroupClasses(int id)
        {
            return GetGroupClasses(id);
        }

        public List<Class> GetUserClasses(int id)
        {
            return GetUserClassesById(id);
        }

        public List<Class> GetPendingConfirmClasses()
        {
            return GetPendingClasses();
        }

        public List<OverlapClassGroup> GroupConflictingClasses(List<Class> classes)
        {
            return GroupOverlappingClasses(classes);
        }

        public ActionResponse ConfirmPendingClass(int id)
        {
            return ConfirmPendingClassById(id);
        }
    }
}
