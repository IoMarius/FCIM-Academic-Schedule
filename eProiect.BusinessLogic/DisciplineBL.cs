using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic
{
     public class DisciplineBL :DisciplineApi ,  IDiscipline
     {
          public GroupOfDisciplines GetAllDisciplines()
          {
               return GetAllDiscipline();
          }
          public ActionResponse AddNewDisciplines(Discipline discipline)
          {
               return AddNewDiscipline(discipline);
          }
          public ActionResponse EditDisciplines(Discipline updateDiscipline)
          {
               return EditDiscipline(updateDiscipline);
          }
          public ActionResponse DeleteDisciplines(int Id)
          {
               return DeleteDiscipline(Id);
          }
     }
}
