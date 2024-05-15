using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.Schedule;
using eProiect.Domain.Entities.User.DBModel;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security;


//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//
//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//
//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//
//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//
//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//
//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//
//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//
//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//
//DELETE PERECHE////DELETE PERECHE////DELETE PERECHE////DELETE PERECHE//


namespace eProiect.BusinessLogic.Core
{
    public class OrgApi
    {
        internal UserSchedule GetUserScheduleById(int Id)
        {
            if (Id < 0 && Id != 0)
                return null;

            User user;
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Id == Id);
            }

            if (user == null)
            {
                return null;
            }

            UserSchedule schedule = new UserSchedule();
            using (var db = new UserContext())
            {
                var classes = db.Classes
                    .Include(c => c.UserDiscipline)
                    .Include(c => c.UserDiscipline.Type)
                    .Include(c => c.AcademicGroup)
                    .Include(c => c.ClassRoom)
                    .Include(c => c.WeekDay)
                    .Include(c => c.UserDiscipline.Discipline)
                    .Include(c => c.UserDiscipline.User)
                    .Where(c => c.UserDiscipline.UserId == user.Id)
                    .ToList();

                foreach (var lesson in classes)
                {
                    schedule.AddLesson(
                        new Lesson(
                            lesson.Id,
                            lesson.UserDiscipline.Discipline.Name,
                            lesson.UserDiscipline.Discipline.ShortName,
                            lesson.UserDiscipline.Type.TypeName,
                            lesson.WeekDay.ShortName,
                            lesson.ClassRoom.ClassroomName,
                            lesson.AcademicGroup.Name,
                            lesson.StartTime,
                            lesson.EndTime,
                            lesson.Frequency
                        )
                    );
                }
            }

            return schedule;
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
                        ((c.StartTime == newClass.StartTime) || (c.EndTime == newClass.EndTime))
                    );
                

                if (overlapWithSelf != null)
                {
                    if(
                        !((overlapWithSelf.UserDiscipline.Type.Id==newClass.UserDiscipline.Type.Id) &&
                        (overlapWithSelf.AcademicGroup.Year==newClass.AcademicGroup.Year))
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
                    .Include(cl=> cl.UserDiscipline.User)
                    .FirstOrDefault(
                        c => c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId == newClass.WeekDay.Id &&
                        c.Frequency == newClass.Frequency &&
                        ((c.StartTime == newClass.StartTime) || (c.EndTime == newClass.EndTime))
                    );

                if (overlapBusyGroup != null) 
                {
                    //get group name
                    var overlapGroup = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                    return new ActionResponse()
                    {
                        ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {newClass.StartTime:hh\\:mm}-{newClass.EndTime:hh\\:mm}",
                        OverlapClassId = overlapBusyGroup.Id,
                        Status = false
                    };
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
                        c.StartTime == midTimeSpan
                    );

                    var endsMidtime = db.Classes.FirstOrDefault(c =>
                        c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId == newClass.WeekDay.Id &&
                        c.EndTime == midTimeSpan
                    );

                    if (startsMidtime != null)//test it 
                    {
                        var overlapGroup = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                        return new ActionResponse()
                        {
                            ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {midTimeSpan:hh\\:mm}-{newClass.EndTime:hh\\:mm}",
                            OverlapClassId = startsMidtime.Id,
                            Status = false
                        };
                    }

                    if (endsMidtime != null)
                    {
                        var overlapGroup = db.AcademicGroups.FirstOrDefault(ag => ag.Id == newClass.AcademicGroup.Id);
                        return new ActionResponse()
                        {
                            ActionStatusMsg = $"Grupa {overlapGroup.Name} este ocupată {newClass.StartTime:hh\\:mm}-{midTimeSpan:hh\\:mm}",
                            OverlapClassId = endsMidtime.Id,
                            Status = false
                        };
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
                    .FirstOrDefault(ud =>  ud.DisciplineId == newClass.UserDiscipline.Id && ud.ClassTypeId ==newClass.UserDiscipline.Type.Id && ud.UserId==newClass.UserDiscipline.User.Id);

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
                        Frequency = newClass.Frequency

                    };
                    db.Classes.Add(veryNewClass);
                    db.SaveChanges();
                }
                else
                {
                    return new ActionResponse { Status = false, ActionStatusMsg = "Internal server error" };
                }
            }


            return new ActionResponse { Status = true , ActionStatusMsg = $"Succes ({groupNameMessage.Name})"};
        }

        internal List<AcademicGroup> GetAcademicGroupsList()
        {
            List<AcademicGroup> groupList;
            using (var db = new UserContext())
            {
                groupList = db.AcademicGroups.ToList();
            }
            if (groupList == null)
                return null;

            return groupList;
        }

        internal List<AcademicGroup> GetAcademicGroupsList(int year)
        {
            List<AcademicGroup> groupList;
            using (var db = new UserContext())
            {
                groupList = db.AcademicGroups
                    .Where(a => a.Year == year)
                    .ToList();
            }
            if (groupList == null)
                return null;

            return groupList;
        }

        internal List<Discipline> GetUserDisciplinesById(int id)
        {
            List<Discipline> discList; //= new List<UserDiscipline>();
            using (var db = new UserContext())
            {
                discList = db.UserDisciplines
                    .Include(ud => ud.Discipline)
                    .Include(ud => ud.User)
                    .Include(ud => ud.Type)
                    .Where(ud => ud.UserId == id)
                    .Select(ud => ud.Discipline)
                    .ToList();
            }
            if (discList == null)
                return new List<Discipline>();

            return discList.Distinct().ToList();
        }

        internal List<ClassType> GetUserDisciplineTypesByUserId(int disciplineId, int userId)
        {
            var types = new List<ClassType>();
            using (var db = new UserContext())
            {
                types = db.UserDisciplines
                    .Where(ud => ud.UserId == userId && ud.DisciplineId == disciplineId)
                    .Select(ud => ud.Type)
                    .ToList();
            }
            return types;
        }

        //UNFINISHED////UNFINISHED////UNFINISHED////UNFINISHED////UNFINISHED////UNFINISHED//
        //DO PAR IMPAR FULL CHECKS
        internal List<ClassRoom> GetClassroomsFreeAtTime(FreeClassroomsRequest data)
        {
            //determine end time by startime + 01:30*span $$ exception for pausa del masa span 2.
            TimeSpan endTime;
            if (data.Span == 2)
            {
                if (data.StartTime == new TimeSpan(11, 30, 0))
                    endTime = data.StartTime + new TimeSpan(3, 30, 0);
                else
                    endTime = data.StartTime + new TimeSpan(3, 15, 0);
            }
            else
            {
                endTime = data.StartTime + new TimeSpan(1, 30, 0);
            }

            List<ClassRoom> freeClassrooms = new List<ClassRoom>();

            using (var db = new UserContext())
            {
                ///MISSING-FREQUENCY//////MISSING-FREQUENCY//////MISSING-FREQUENCY//////MISSING-FREQUENCY//////MISSING-FREQUENCY///

                //scot toate cabinetele de pe etaj
               var classRoomsonFloor = db.ClassRooms.Where(cr => cr.Floor == data.Floor).ToList();

                //caut cabinetele ocupate la ora dată //fixit
                var busyClassrooms = db.Classes.Where(cl =>
                    cl.WeekDay.Id == data.WeekdayId &&
                    ((cl.StartTime == data.StartTime) || (cl.EndTime == endTime))
                ).Select(cl => cl.ClassRoom).ToList();

                //check span two for overlap (midSpan end start)
                if (data.Span == 2)
                {
                    TimeSpan midTimeSpan;
                    //check pausa del masa
                    if (data.StartTime == new TimeSpan(11, 30, 0))
                    {
                        midTimeSpan = endTime - new TimeSpan(2, 0, 0);  //30 + 30 min accounts for pauză de masa
                    }
                    else
                    {
                        midTimeSpan = endTime - new TimeSpan(1, 45, 0); //30 + 15 min accounts for pauză
                    }

                    var ClassBusyMidtimeStart = db.Classes.Where(cl =>
                            cl.WeekDay.Id == data.WeekdayId &&
                            cl.StartTime == midTimeSpan
                        ).Select(cl => cl.ClassRoom).ToList();

                    var ClassBusyMidtimeEnd = db.Classes.Where(cl =>
                            cl.WeekDay.Id == data.WeekdayId &&
                            cl.EndTime == midTimeSpan
                        ).Select(cl => cl.ClassRoom).ToList();


                    busyClassrooms.AddRange(ClassBusyMidtimeStart);
                    busyClassrooms.AddRange(ClassBusyMidtimeEnd);
                }

                //scad din toate cele ocupate 
                freeClassrooms = classRoomsonFloor.Where(x => !busyClassrooms.Contains(x)).ToList();
            }

            return freeClassrooms;
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

                    if (startsMidtime != null || endsMidtime!=null)
                    {            
                        return new ActionResponse()
                        {
                            Status = false,
                            ActionStatusMsg = "Grupa academică este ocupată în timpul selectat",
                            OverlapClassId= startsMidtime!=null? startsMidtime.Id:endsMidtime.Id
                        };
                    }
                }
            }

            //edit in db
            using(var db=new UserContext())
            {
                var underEditClass = db.Classes
                    .Include(cl => cl.ClassRoom)
                    .Include(cl => cl.WeekDay)
                    .FirstOrDefault(cl => cl.Id == editedClass.Id);

                if (underEditClass is null) {
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

                var weekdayEdit=db.WeekDays
                    .FirstOrDefault(wd=>wd.Id == editedClass.WeekDay.Id);

                if(classroomEdit is null || weekdayEdit is null) {
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

            return new ActionResponse() {Status=true, ActionStatusMsg="Salvat"};
        }

        internal Class GetClass(int id)
        {
            Class result;
            using(var db=new UserContext())
            {
                result = db.Classes
                    .Include(cl => cl.ClassRoom)
                    .Include(cl => cl.AcademicGroup)
                    .Include(cl => cl.WeekDay)
                    .Include(cl => cl.UserDiscipline)
                    .Include(cl => cl.UserDiscipline.Type)
                    .Include(cl => cl.UserDiscipline.Discipline)
                    .FirstOrDefault(cl=>cl.Id==id);
            }
            return result;
        }

        internal ActionResponse RemoveUserClassById(int id)
        {
            Class toDelete;
            using (var db=new UserContext())
            {
                toDelete=db.Classes.FirstOrDefault(cl=>cl.Id==id);
            }
            
            if(toDelete is null) {
                return new ActionResponse()
                {
                    Status = false,
                    ActionStatusMsg = "Pereche inexistentă"
                };
            }

            using (var db=new UserContext())
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
            var groupClasses=new List<Class>();
            using (var db=new UserContext())
            {
                groupClasses = db.Classes
                    .Include(cl => cl.UserDiscipline.User)
                    .Include(cl => cl.UserDiscipline.Type)                    
                    .Include(cl => cl.UserDiscipline.Discipline)                    
                    .Include(cl => cl.ClassRoom)                    
                    .Include(cl => cl.WeekDay)                    
                    .Where(cl => cl.AcademicGroupId==id)
                    .ToList();
            }

            //sort by weekdayId asc.
            return groupClasses
                .OrderBy(cl=>cl.WeekDayId)
                .ThenBy(cl=>cl.StartTime)
                .ToList();
        }

        internal List<User> GetAllTeacherUsers()
        {
            var users = new List<User>();
            using(var db=new UserContext())
            {
                users = db.UserDisciplines
                    .Select(Ud => Ud.User)
                    .Distinct()
                    .ToList();
            }
            return users;
        }

        internal List<Class> GetUserClassesById(int id)
        {
            var classes = new List<Class>();
            using(var db=new UserContext())
            {
                classes = db.Classes
                    .Include(cl => cl.UserDiscipline.User)
                    .Include(cl => cl.UserDiscipline.Type)
                    .Include(cl => cl.UserDiscipline.Discipline)
                    .Include(cl => cl.ClassRoom)
                    .Include(cl => cl.WeekDay)
                    .Include(cl => cl.AcademicGroup)
                    .Where(cl => cl.UserDiscipline.UserId == id)
                    .ToList();
            }
            return classes;
        }

        internal bool IsEvenWeek()
        {            
            DateTime today = DateTime.Today;
            DayOfWeek currentDayOfWeek = today.DayOfWeek;

            int daysToSubtract = ((int)currentDayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            var mondayDate = today.AddDays(-daysToSubtract);
            
            if (mondayDate.Day % 2 == 0)
                return true;
            return false;                        
        }
    }
}
