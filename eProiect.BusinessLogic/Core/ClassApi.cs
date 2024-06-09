using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.BusinessLogic.Migrations;
using eProiect.Helper;

namespace eProiect.BusinessLogic.Core
{
    public class ClassApi
    {
        private void NotifyUsersAboutChange(int groupId)
        {
            using (var db = new UserContext())
            {
                var academicGroup = db.AcademicGroups
                .FirstOrDefault(ag => ag.Id == groupId);


                if (academicGroup == null)
                    return;

                var emails =  db.Students
                    .Where(st => st.AcademicGroupId == groupId)
                    .Select(st => st.Email)
                    .ToList();

                var subNotifier = new SubscriberNotifier();
                subNotifier.NotifySubscribersAboutGroupChange(
                    emails,
                    academicGroup.Name
                );
            }
        }

        internal ActionResponse EditClass(Class editedClass)
        {
            if (editedClass == null)
            {
                return new ActionResponse() { Status = false, ActionStatusMsg = "Interal error" };
            }

            //checkups
            using (var db = new UserContext())
            {
                //overlap with self    
                var selfOverlap = db.Classes
                    .Include(c => c.UserDiscipline)
                    .Include(c => c.WeekDay)
                    .Include(c => c.UserDiscipline.User)
                    .Include(c => c.UserDiscipline.Type)
                    .Include(c => c.UserDiscipline.Discipline)
                    .Include(c => c.AcademicGroup)
                    .FirstOrDefault(c =>
                        c.UserDiscipline.User.Id == editedClass.UserDiscipline.User.Id &&
                        c.WeekDay.Id == editedClass.WeekDay.Id &&
                        c.Id != editedClass.Id &&
                        ((c.StartTime == editedClass.StartTime) || (c.EndTime == editedClass.EndTime))
                    );

                if (selfOverlap != null)
                {
                    return new ActionResponse()
                    {
                        Status = false,
                        ActionStatusMsg = $"Suneți ocupat în perioada selectată: {selfOverlap.UserDiscipline.Type.TypeName} {selfOverlap.UserDiscipline.Discipline.Name}, {selfOverlap.AcademicGroup.Name}"
                    };
                }

                //overlap with group
                var groupOverlap = db.Classes
                    .FirstOrDefault(
                        c => c.AcademicGroupId == editedClass.AcademicGroup.Id &&
                        c.WeekDayId == editedClass.WeekDay.Id &&
                        c.Id != editedClass.Id &&
                        ((c.StartTime == editedClass.StartTime) || (c.EndTime == editedClass.EndTime))
                    );

                if (groupOverlap != null)
                {
                    return new ActionResponse()
                    {
                        Status = false,
                        ActionStatusMsg = "Grupa academică este ocupată în timpul selectat",
                        OverlapClassId = groupOverlap.Id
                    };
                }

                //midtime overlap with double span classes
                if (editedClass.EndTime - editedClass.StartTime != new TimeSpan(1, 30, 0))
                {

                    TimeSpan midTimeSpan;
                    //check pausa del masa
                    if (editedClass.StartTime == new TimeSpan(11, 30, 0))
                    {
                        midTimeSpan = editedClass.EndTime - new TimeSpan(2, 0, 0);
                    }
                    else
                    {
                        midTimeSpan = editedClass.EndTime - new TimeSpan(1, 45, 0); //30 + 15 min accounts for pauză
                    }

                    var startsMidtime = db.Classes
                        .Include(c => c.AcademicGroup)
                        .Include(c => c.UserDiscipline)
                        .Include(c => c.UserDiscipline.Type)
                        .Include(c => c.UserDiscipline.Discipline)
                        .Include(c => c.UserDiscipline.User)
                        .FirstOrDefault(c =>
                        c.AcademicGroupId == editedClass.AcademicGroup.Id &&
                        c.WeekDayId == editedClass.WeekDay.Id &&
                        c.StartTime == midTimeSpan &&
                        c.Id != editedClass.Id
                    );

                    var endsMidtime = db.Classes
                        .Include(c => c.AcademicGroup)
                        .Include(c => c.UserDiscipline)
                        .Include(c => c.UserDiscipline.Type)
                        .Include(c => c.UserDiscipline.Discipline)
                        .Include(c => c.UserDiscipline.User)
                        .FirstOrDefault(c =>
                        c.AcademicGroupId == editedClass.AcademicGroup.Id &&
                        c.WeekDayId == editedClass.WeekDay.Id &&
                        c.EndTime == midTimeSpan &&
                        c.Id != editedClass.Id
                    );

                    if (startsMidtime != null || endsMidtime != null)
                    {
                        return new ActionResponse()
                        {
                            Status = false,
                            ActionStatusMsg = "Grupa academică este ocupată în timpul selectat",
                            OverlapClassId = startsMidtime != null ? startsMidtime.Id : endsMidtime.Id
                        };
                    }
                }
            }

            //edit in db
            using (var db = new UserContext())
            {
                var underEditClass = db.Classes
                    .Include(cl => cl.ClassRoom)
                    .Include(cl => cl.WeekDay)
                    .FirstOrDefault(cl => cl.Id == editedClass.Id);

                if (underEditClass is null)
                {
                    return new ActionResponse()
                    {
                        Status = false,
                        ActionStatusMsg = "Internal error"
                    };
                }

                //get objects with corresponding ID's
                //classroom
                var classroomEdit = db.ClassRooms
                    .FirstOrDefault(cr => cr.Id == editedClass.ClassRoom.Id);

                var weekdayEdit = db.WeekDays
                    .FirstOrDefault(wd => wd.Id == editedClass.WeekDay.Id);

                if (classroomEdit is null || weekdayEdit is null)
                {
                    return new ActionResponse()
                    {
                        Status = false,
                        ActionStatusMsg = "Internal error"
                    };
                }


                underEditClass.ClassRoom = classroomEdit;
                underEditClass.WeekDay = weekdayEdit;
                underEditClass.StartTime = editedClass.StartTime;
                underEditClass.EndTime = editedClass.EndTime;
                underEditClass.Frequency = editedClass.Frequency;


                db.Entry(underEditClass).State = EntityState.Modified;
                db.SaveChanges();
            }

            NotifyUsersAboutChange(editedClass.AcademicGroupId);

            return new ActionResponse() { Status = true, ActionStatusMsg = "Salvat" };
        }

        internal ActionResponse AddNewClassToDb(Class newClass)
        {
            if (newClass == null)
            {
                System.Diagnostics.Debug.WriteLine("AddNewClassToDb(Class): New class cannot be null.");
                return new ActionResponse()
                {
                    Status = false,
                    ActionStatusMsg = "Internal error"
                };
            }
            
            try
            {             
                //inserting the new class in the database.
                using (var db = new UserContext())
                {
                    var userDiscipline = db.UserDisciplines
                        .Include(ud => ud.User)
                        .Include(ud => ud.Discipline)
                        .Include(ud => ud.Type)
                        .FirstOrDefault(ud => ud.DisciplineId == newClass.UserDiscipline.Id && ud.ClassTypeId == newClass.UserDiscipline.Type.Id && ud.UserId == newClass.UserDiscipline.User.Id);

                    var classType = db.ClassTypes
                        .FirstOrDefault(ud => ud.Id == newClass.UserDiscipline.Type.Id);

                    var academicGroup = db.AcademicGroups
                        .FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);

                    var classRoom = db.ClassRooms
                        .FirstOrDefault(cr => cr.Id == newClass.ClassRoom.Id);

                    var weekDay = db.WeekDays
                        .FirstOrDefault(wd => wd.Id == newClass.WeekDay.Id);
                    userDiscipline.Type = classType;

                    if (userDiscipline != null && academicGroup != null && classRoom != null && weekDay != null)
                    {

                        var veryNewClass = new Class
                        {
                            UserDiscipline = userDiscipline,
                            AcademicGroup = academicGroup,
                            ClassRoom = classRoom,
                            WeekDay = weekDay,
                            StartTime = newClass.StartTime,
                            EndTime = newClass.EndTime,
                            Frequency = newClass.Frequency,
                            IsConfirmed = false
                        };
                        db.Classes.Add(veryNewClass);
                        db.SaveChanges();
                    }
                    else
                    {
                        return new ActionResponse { Status = false, ActionStatusMsg = "Internal error" };
                    }

                }

                return new ActionResponse { Status = true, ActionStatusMsg="Success"};
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddNewClassToDb(Class) Exception caught:{ex.Message}\n Details:{ex.InnerException}");
                return new ActionResponse() {
                    Status = false,
                    ActionStatusMsg = "Internal error"
                };
            }


        }

        internal Class GetClass(int id)
        {
            Class result;
            using (var db = new UserContext())
            {
                result = db.Classes
                    .Include(cl => cl.ClassRoom)
                    .Include(cl => cl.AcademicGroup)
                    .Include(cl => cl.WeekDay)
                    .Include(cl => cl.UserDiscipline)
                    .Include(cl => cl.UserDiscipline.Type)
                    .Include(cl => cl.UserDiscipline.Discipline)
                    .Include(cl => cl.UserDiscipline.User)
                    .FirstOrDefault(cl => cl.Id == id);
            }
            return result;
        }

        internal List<Class> GetUserClassesById(int id)
        {                        
            var classes = new List<Class>();
            using (var db = new UserContext())
            {
                classes = db.Classes
                    .Include(cl => cl.UserDiscipline.User)
                    .Include(cl => cl.UserDiscipline.Type)
                    .Include(cl => cl.UserDiscipline.Discipline)
                    .Include(cl => cl.ClassRoom)
                    .Include(cl => cl.WeekDay)
                    .Include(cl => cl.AcademicGroup)
                    .Where(cl => cl.UserDiscipline.UserId == id && cl.IsConfirmed == true)
                    .ToList();
            }
            return classes;
        }

        internal ActionResponse RemoveUserClassById(int id)
        {
            Class toDelete;
            using (var db = new UserContext())
            {
                toDelete = db.Classes.FirstOrDefault(cl => cl.Id == id);
            }

            if (toDelete is null)
            {
                return new ActionResponse()
                {
                    Status = false,
                    ActionStatusMsg = "Pereche inexistentă"
                };
            }

            using (var db = new UserContext())
            {
                db.Classes.Attach(toDelete);
                db.Classes.Remove(toDelete);
                db.SaveChanges();
            }

            NotifyUsersAboutChange(toDelete.AcademicGroupId);

            return new ActionResponse()
            {
                Status = true,
                ActionStatusMsg = "Perechea a fost ștearsă cu succes"
            };
        }

        internal List<Class> GetGroupClasses(int id)
        {
            var groupClasses = new List<Class>();
            using (var db = new UserContext())
            {
                groupClasses = db.Classes
                    .Include(cl => cl.UserDiscipline.User)
                    .Include(cl => cl.UserDiscipline.Type)
                    .Include(cl => cl.UserDiscipline.Discipline)
                    .Include(cl => cl.ClassRoom)
                    .Include(cl => cl.WeekDay)
                    .Where(cl => cl.AcademicGroupId == id && cl.IsConfirmed == true)
                    .ToList();
            }

            //sort by weekdayId asc.
            return groupClasses
                .OrderBy(cl => cl.WeekDayId)
                .ThenBy(cl => cl.StartTime)
                .ToList();
        }

        internal List<Class> GetClassesPending()
        {
            try
            {
                using(var db = new UserContext())
                {
                    var pending = db.Classes
                        .Include(cl=>cl.UserDiscipline)
                        .Include(cl=>cl.UserDiscipline.User)
                        .Include(cl=>cl.UserDiscipline.Discipline)
                        .Include(cl=>cl.UserDiscipline.Type)
                        .Include(cl=>cl.AcademicGroup)
                        .Include(cl=>cl.ClassRoom)
                        .Include(cl=>cl.WeekDay)
                        .Where(cl => !cl.IsConfirmed).ToList();
                    return pending;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetPendingClasses(Class) Exception caught:{ex.Message}");
                return new List<Class>();
            }
        }

        internal List<ConflictGroup> GetConflictingClasses()
        {
            try { 
                using(var db = new UserContext())
                {
                    var classes = db.Classes
                        .Include(cl=>cl.AcademicGroup)
                        .Include(cl=>cl.ClassRoom)
                        .Include(cl=>cl.UserDiscipline)
                        .Include(cl=>cl.UserDiscipline.User)
                        .Include(cl=>cl.UserDiscipline.Discipline)
                        .Include(cl=>cl.UserDiscipline.Type)
                        .Include(cl=>cl.WeekDay)
                        .ToList();

                    var groupedClasses = classes.SelectMany(cls =>
                        classes.Where(ocls =>
                            cls.Id != ocls.Id &&
                            ((cls.AcademicGroup.Id == ocls.AcademicGroup.Id)||(cls.ClassRoom == ocls.ClassRoom)) &&
                            cls.WeekDay.Id == ocls.WeekDay.Id &&
                            cls.StartTime < ocls.EndTime &&
                            cls.EndTime > ocls.StartTime &&
                            cls.UserDiscipline.User.Id != ocls.UserDiscipline.User.Id
                        ).Select(ocls => new { cls, ocls }))
                        .GroupBy(pair => pair.cls)
                        .Select(group => new
                        {
                            Class = group.Key,
                            OverlappingClasses = group.Select(x => x.ocls).ToList()
                        })
                        .ToList();

                    var result = new List<ConflictGroup>();
                    foreach(var group in groupedClasses)
                    {
                        /*var conflictList = group.OverlappingClasses.ToList();*/
                        //conflictList.Add(group.Class);

                        result.Add(new ConflictGroup()
                        {
                            MainClass = group.Class,
                            OverlappingClasses = group.OverlappingClasses.ToList()
                        });
                    }
                           
                  // db.SaveChanges();                                         

                    return result;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetPendingClasses(Class) Exception caught:{ex.Message}");
                return null;
            }

            /*var classes = new List<Class>();
            using (var db = new UserContext())
            {
                classes = db.Classes
                    .Include(cl => cl.UserDiscipline.User)
                    .Include(cl => cl.UserDiscipline.Type)
                    .Include(cl => cl.UserDiscipline.Discipline)
                    .Include(cl => cl.ClassRoom)
                    .Include(cl => cl.WeekDay)
                    .Include(cl => cl.AcademicGroup)
                    .Where(cl => cl.IsConfirmed == false).ToList();
            }
            return classes;*/
        }

        internal List<OverlapClassGroup> GroupOverlappingClasses(List<Class> classes)
        {
            var overlapList = new List<OverlapClassGroup>();
            foreach (var cls in classes.ToList())
            {


                var overlaps = new OverlapClassGroup();


                overlaps.OverlapGroup.Add(cls);
                classes.Remove(cls);

                foreach (var otherCls in classes.ToList())
                {
                    if (
                        //overlap with academic group
                        (/*cls.AcademicGroup.Id == otherCls.AcademicGroup.Id
                        && cls.UserDiscipline.Discipline.Id == otherCls.UserDiscipline.Discipline.Id
                        && cls.UserDiscipline.Type.Id == otherCls.UserDiscipline.Type.Id*/

                        cls.WeekDay.Id == otherCls.WeekDay.Id
                        && cls.Frequency == otherCls.Frequency
                        && (cls.StartTime >= otherCls.StartTime && cls.EndTime <= otherCls.EndTime))
                        && (cls.ClassRoom.Id == otherCls.ClassRoom.Id || cls.AcademicGroup.Id == otherCls.AcademicGroup.Id)
                    )
                    {
                        overlaps.OverlapGroup.Add(otherCls);
                        classes.Remove(otherCls);
                    }
                }
                overlapList.Add(overlaps);
            }


            /*var overlapList = new List<OverlapClassGroup>();
            var classesCount = classes.Count;
            for(int i=0; i< classesCount; ++i)
            {
                if (classes[i] == null)                
                    continue;

                var overlaps = new OverlapClassGroup();
                
                var cls = classes[i];
                overlaps.OverlapGroup.Add(cls);
                classes.Remove(cls);
           
                foreach(var otherCls in classes.ToList())
                {
                    if (
                        //overlap with academic group
                        (*//*cls.AcademicGroup.Id == otherCls.AcademicGroup.Id
                        && cls.UserDiscipline.Discipline.Id == otherCls.UserDiscipline.Discipline.Id
                        && cls.UserDiscipline.Type.Id == otherCls.UserDiscipline.Type.Id*//*
                        cls.WeekDay.Id == otherCls.WeekDay.Id
                        && cls.Frequency==otherCls.Frequency
                        && (cls.StartTime >= otherCls.StartTime && cls.EndTime <= otherCls.EndTime))
                        && cls.ClassRoom.Id == otherCls.ClassRoom.Id
                    )
                    {
                        overlaps.OverlapGroup.Add(otherCls);
                        classes.Remove(otherCls);
                    }
                }
                overlapList.Add(overlaps);
            }*/


            /*for(var i=0; i<classes.Count(); ++i)
            {
                var cls = classes[i];
                var overlapGroup = new OverlapClassGroup();
                
                for(var j =0; j<otherClasses.Count(); ++j)
                {
                    var otherCls = otherClasses[j];
                    //add current class as first element
                    overlapGroup.OverlapGroup.Add(cls);
                    if(
                        //overlap with academic group
                        (cls.AcademicGroup.Id==otherCls.AcademicGroup.Id 
                        && cls.UserDiscipline.Discipline.Id == otherCls.UserDiscipline.Discipline.Id
                        && cls.UserDiscipline.Type.Id == otherCls.UserDiscipline.Type.Id
                        && cls.WeekDay.Id == otherCls.WeekDay.Id
                        && (cls.StartTime>=otherCls.StartTime && cls.EndTime<=otherCls.EndTime))

                    ) {
                        //add identified overlap to group
                        overlapGroup.OverlapGroup.Add(otherCls);
                        
                        //remove identified overlap from otherCls
                        otherClasses.Remove(otherCls);
                    }
                }
                overlapList.Add(overlapGroup);
            }*/





            return overlapList;
        }

        internal ActionResponse ConfirmPendingClassById(int id)
        {
            try
            {
                using (var db = new UserContext())
                {
                    var cls = db.Classes.FirstOrDefault(cl => cl.Id == id && !cl.IsConfirmed);
                    if (cls != null)
                    {
                        cls.IsConfirmed = true;
                        db.SaveChanges();

                        NotifyUsersAboutChange(cls.AcademicGroupId);
                        
                        return new ActionResponse { Status = true };
                    }
                    else
                    {
                        return new ActionResponse
                        {
                            Status = false,
                            ActionStatusMsg = $"Pending class with id={id} not found."
                        };
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ActionResponse
                {
                    Status = false,
                    ActionStatusMsg = $"Concurrency error updating class with id={id}. Details: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ActionResponse
                {
                    Status = false,
                    ActionStatusMsg = $"An errror occured confirming the class. Details: {ex.Message}"
                };
            }
        }

        internal List<Class> GetClassroomScheduleAction(int id)
        {
            try 
            {   
                var classes = new List<Class>();
                using(var db = new UserContext())
                {
                    classes = db.Classes
                        .Include(cl => cl.ClassRoom)
                        .Include(cl => cl.AcademicGroup)
                        .Include(cl => cl.WeekDay)
                        .Include(cl => cl.UserDiscipline)
                        .Include(cl => cl.UserDiscipline.Type)
                        .Include(cl => cl.UserDiscipline.Discipline)
                        .Include(cl => cl.UserDiscipline.User)
                        .Where(cl=>cl.ClassRoom.Id==id)
                        .ToList();
                }
                return classes;
            }
            catch(Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine($"GetClassroomScheduleAction(int): Exception caught {ex.Message}\n Details: {ex.InnerException}, Stack Trace: {ex.StackTrace}");
                return new List<Class>();
            }
        }
    }
}
