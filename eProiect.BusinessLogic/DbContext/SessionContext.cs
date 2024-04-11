using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.DBContext
{
    public class SessionContext : DbContext
    {
        public SessionContext() :
            base("name=eProiect1")
        {
            Database.SetInitializer<SessionContext>(null);
        }

        public virtual DbSet<Session> Sessions { get; set; }
    }
}
