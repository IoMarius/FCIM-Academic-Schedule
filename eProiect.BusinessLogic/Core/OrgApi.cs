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
                    ActionStatusMsg = "Eroare !"
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
                    .FirstOrDefault(
                        c => c.AcademicGroupId == newClass.AcademicGroup.Id &&
                        c.WeekDayId == newClass.WeekDay.Id &&
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
                    .FirstOrDefault(ud =>  ud.DisciplineId == newClass.UserDiscipline.Id && ud.ClassTypeId ==newClass.UserDiscipline.Type.Id);//does not work

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
                    return new ActionResponse { Status = false, ActionStatusMsg = "Internal server error." };
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
    }
}
