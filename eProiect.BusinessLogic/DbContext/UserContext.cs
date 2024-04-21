using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using eProiect.BusinessLogic.DBContext;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Schedule;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;

namespace eProiect.BusinessLogic.DBModel
{
    public class UserContext : DbContext
    {
        public UserContext() :
            base("name=eProiect1") //base connection
        {
            Database.SetInitializer<UserContext>(null);
        }


        //modelul contextului
        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<UserCredential> UserCredentials { set; get; }
        public virtual DbSet<UserDiscipline> UserDisciplines { set; get; }


        public virtual DbSet<WeekDay> WeekDays { set; get; }
        public virtual DbSet<Class> Classes { set; get; }



        public virtual DbSet<ClassRoom> ClassRooms { set; get; }
        public virtual DbSet<Discipline> Disciplines { set; get; }
        public virtual DbSet<ClassType> ClassTypes { set; get; }
        public virtual DbSet<Students> Students { set; get; }
        public virtual DbSet<AcademicGroup> AcademicGroups { set; get; }
     

     }    
}
