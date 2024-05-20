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

namespace eProiect.BusinessLogic.Core
{
    public class ClassApi
    {
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

            return new ActionResponse() { Status = true, ActionStatusMsg = "Salvat" };
        }

        internal ActionResponse AddNewClassToDb(Class newClass)
        {
            if (newClass == null)
            {
                return new ActionResponse()
                {
                    Status = false,
                    ActionStatusMsg = "Internal error."
                };
            }

            string responseMsg = $"Succes";
            //to check before insertion !!
            // free classroom +
            // free group at given time (do not forget span) + 

            using (var db = new UserContext())
            {

                //check overlap with own schedule, if busy as a human.
                var overlapWithSelf = db.Classes
                    .Include(c => c.UserDiscipline)
                    .Include(c => c.WeekDay)
                    .Include(c => c.UserDiscipline.User)
                    .Include(c => c.UserDiscipline.Type)
                    .Include(c => c.AcademicGroup)
                    .FirstOrDefault(c =>
                        c.UserDiscipline.User.Id == newClass.UserDiscipline.User.Id &&
                        c.WeekDay.Id == newClass.WeekDay.Id &&
                        c.IsConfirmed == true &&
                        ((c.StartTime == newClass.StartTime) || (c.EndTime == newClass.EndTime))
                    );


                if (overlapWithSelf != null)
                {
                    if (
                        !((overlapWithSelf.UserDiscipline.Type.Id == newClass.UserDiscipline.Type.Id) &&
                        (overlapWithSelf.AcademicGroup.Year == newClass.AcademicGroup.Year))
                      )
                    {
                        return new ActionResponse()
                        {
                            ActionStatusMsg = $"Deja există perechi în acest interval: {newClass.StartTime:hh\\:mm}-{newClass.EndTime:hh\\:mm}",
                            OverlapClassId = overlapWithSelf.Id,
                            Status = false
                        };
                    }
                }

                var overlapBusyGroup = db.Classes
                    .Include(cl => cl.UserDiscipline.User)
                    .FirstOrDefault(
                        c => c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId == newClass.WeekDay.Id &&
                        c.Frequency == newClass.Frequency &&
                        c.IsConfirmed == true &&
                        ((c.StartTime == newClass.StartTime) || (c.EndTime == newClass.EndTime))
                    );

                if (overlapBusyGroup != null)
                {
                    //get group name
                    var overlapGroup = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                    /* return new ActionResponse()
                     {
                         ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {newClass.StartTime:hh\\:mm}-{newClass.EndTime:hh\\:mm}",
                         OverlapClassId = overlapBusyGroup.Id,
                         Status = false
                     };*/
                    responseMsg = $"Grupa {overlapGroup.Name} este ocupată {newClass.StartTime:hh\\:mm}-{newClass.EndTime:hh\\:mm}";
                }


                //check mid time span for long classes (add to cabinete Too)
                if (newClass.EndTime - newClass.StartTime != new TimeSpan(1, 30, 0))
                {

                    TimeSpan midTimeSpan;
                    //check pausa del masa
                    if (newClass.StartTime == new TimeSpan(11, 30, 0))
                    {
                        midTimeSpan = newClass.EndTime - new TimeSpan(2, 0, 0);
                    }
                    else
                    {
                        midTimeSpan = newClass.EndTime - new TimeSpan(1, 45, 0); //30 + 15 min accounts for pauză
                    }

                    var startsMidtime = db.Classes.FirstOrDefault(c =>
                        c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId == newClass.WeekDay.Id &&
                        c.IsConfirmed == true &&
                        c.StartTime == midTimeSpan
                    );

                    var endsMidtime = db.Classes.FirstOrDefault(c =>
                        c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId == newClass.WeekDay.Id &&
                        c.IsConfirmed == true &&
                        c.EndTime == midTimeSpan
                    );

                    if (startsMidtime != null)//test it 
                    {
                        var overlapGroup = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                        /*return new ActionResponse()
                        {
                            ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {midTimeSpan:hh\\:mm}-{newClass.EndTime:hh\\:mm}",
                            OverlapClassId = startsMidtime.Id,
                            Status = false
                        };*/
                        responseMsg = $"Grupa {overlapGroup.Name} este ocupată {midTimeSpan:hh\\:mm}-{newClass.EndTime:hh\\:mm}";
                    }

                    if (endsMidtime != null)
                    {
                        var overlapGroup = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                        /*return new ActionResponse()
                        {
                            ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {newClass.StartTime:hh\\:mm}-{midTimeSpan:hh\\:mm}",
                            OverlapClassId = endsMidtime.Id,
                            Status = false
                        };*/
                        responseMsg = $"Grupa {overlapGroup.Name} este ocupată {newClass.StartTime:hh\\:mm}-{midTimeSpan:hh\\:mm}";
                    }
                }
            }

            AcademicGroup groupNameMessage;
            //insert stage
            using (var db = new UserContext())
            {
                groupNameMessage = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);

                //get tables UserDiscipline, AcademicGroup, ClassRoom, Weekday, types
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
                    return new ActionResponse { Status = false, ActionStatusMsg = "Internal server error" };
                }
            }


            return new ActionResponse { Status = true, ActionStatusMsg = responseMsg };
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

        internal List<Class> GetPendingClasses()
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
                    .Where(cl => cl.IsConfirmed == false).ToList();
            }
            return classes;
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
    }
}
