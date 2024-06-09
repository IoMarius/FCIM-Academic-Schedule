using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.Academic.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic
{
     public class BuissinesLogic
     {
          public ISession GetSessionBL()
          {
               return new SessionBL();
          }

          public IOrg GetOrgBl()
          {
                return new OrgBL();
          }

          public IAdmin GetAdminBL()
          {
               return new AdminBL();
          }
          public IAcademicGroup GetAcademicGroupBL()
          {
               return new AcademicGroupBL();
          }
          public IDiscipline GetDisciplineBL()
          {
               return new DisciplineBL();
          }
          public IClassRoom GetClassRoomBL()
          {
               return new ClassRoomBL();
          }
          public IUserDiscipline GetUserDisciplineBL()
        {
            return new UserDisciplineBLL();
        }

          public IClass GetClassBL()
        {
            return new ClassBL();
        }

          public IUser GetUserBL()
        {
            return new UserBL();
        }

     }
}
