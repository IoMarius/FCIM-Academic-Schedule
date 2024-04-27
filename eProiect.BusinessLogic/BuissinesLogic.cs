using eProiect.BusinessLogic.Interfaces;
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
     }
}
