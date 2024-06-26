﻿using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.Schedule;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.DBContext
{
    public class FatherContext : DbContext
    {
        public FatherContext() :
            base("name=eProiect")
        {
        }

        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<UserCredential> UserCredentials { set; get; }
        public virtual DbSet<UserDiscipline> UserDisciplines { set; get; }


        public virtual DbSet<WeekDay> WeekDays { set; get; }
        public virtual DbSet<Class> Classes { set; get; }
        public virtual DbSet<ConflictingClass> ClassConflicts { get; set; }


        public virtual DbSet<ClassRoom> ClassRooms { set; get; }
        public virtual DbSet<Discipline> Disciplines { set; get; }
        public virtual DbSet<ClassType> ClassTypes { set; get; }
        public virtual DbSet<Students> Students { set; get; }
        public virtual DbSet<AcademicGroup> AcademicGroups { set; get; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<UserResetPassword> UserResetPasswords { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConflictingClass>()
                .HasRequired(cc => cc.MainClass)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConflictingClass>()
                .HasRequired(cc => cc.ConflictingWith)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
