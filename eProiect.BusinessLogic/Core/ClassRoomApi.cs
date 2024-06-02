using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.Domain.Entities.Schedule;

namespace eProiect.BusinessLogic.Core
{
     public class ClassRoomApi
     {
          internal GroupOfCassRooms GetAllClassRoom()
          {
               var allCassRooms = new GroupOfCassRooms();

               using (var db = new UserContext())
               {
                    allCassRooms.CassRooms = db.ClassRooms.ToList();
               }
               return allCassRooms;
          }
          internal ActionResponse AddNewClassRoom(ClassRoom classRoom)
          {
               var newClassRoom = new ClassRoom
               {
                    ClassroomName = classRoom.ClassroomName,
                    Floor = classRoom.Floor
               };

               using (var db = new UserContext())
               {

                    try
                    {
                         db.ClassRooms.Add(newClassRoom);
                         db.SaveChanges();

                    }
                    catch (DbUpdateException ex)
                    {
                         if (ex.InnerException != null)
                         {

                              // Check if inner exception is SqlException
                              if (ex.InnerException is SqlException sqlEx)
                              {
                                   // Handle SqlException
                                   if (sqlEx.Number == 2601) // SQL Server error number for unique constraint violation
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Duplicate key error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                                   else
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Other SQL Server error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                              }
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Inner Exception: " + ex.InnerException.Message,
                                   Status = false
                              };
                         }
                         return new ActionResponse
                         {
                              ActionStatusMsg = "DbUpdateException occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    catch (Exception ex)
                    {
                         // Handle other exceptions
                         return new ActionResponse
                         {
                              ActionStatusMsg = "An error occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Class room was successfully added.",
                         Status = true
                    };
               }
          }
          internal ActionResponse EditClassRoom(ClassRoom updatedClassRoomData)
          {
               try
               {
                    using (var db = new UserContext())
                    {
                         var classRoom = db.ClassRooms.FirstOrDefault(c => c.Id == updatedClassRoomData.Id);
                         if (classRoom == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Classroom not found or invalid ID",
                                   Status = false
                              };
                         }

                         // Update classroom properties
                         classRoom.ClassroomName = updatedClassRoomData.ClassroomName;
                         classRoom.Floor = updatedClassRoomData.Floor;

                         db.SaveChanges();
                    }

                    return new ActionResponse
                    {
                         ActionStatusMsg = "Classroom data updated successfully",
                         Status = true
                    };
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating classroom data: {ex.Message}",
                         Status = false
                    };
               }
          }
          internal ActionResponse DeleteClassRoom(int Id)
          {
               if (Id < 0 && Id != 0)
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Invalid ID",
                         Status = false
                    };
               try
               {
                    ClassRoom _classRoom;
                    using (var db = new UserContext())
                    {
                         _classRoom = db.ClassRooms.FirstOrDefault(c => c.Id == Id);
                         if (_classRoom == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Nu a fost gasita ClassRoom sau este id invalid",
                                   Status = false
                              };
                         }

                         db.ClassRooms.Remove(_classRoom);
                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"A aparut o eroare la stergerea ClassRoom: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "ClassRoom a fost stearsa cu succes",
                    Status = true
               };
          }
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

                freeClassrooms = db.ClassRooms
                    .Where(cl => cl.Floor == data.Floor)
                    .Except(
                        db.Classes
                            .Where(cl =>
                                cl.ClassRoom.Floor == data.Floor &&
                                cl.Frequency == data.Frequency &&
                                cl.WeekDay.Id == data.WeekdayId &&
                                ((cl.StartTime >= data.StartTime) || (cl.EndTime <= endTime))
                            )
                            .Select(cl => cl.ClassRoom)
                            .Distinct()
                        )
                    .ToList();               
            }

            return freeClassrooms;
        }
          
        internal List<ClassRoom> GetClassroomsByFloor(int floor)
        {
            try
            {
                using(var db = new UserContext())
                {
                    var classrooms = db.ClassRooms.Where(cr => cr.Floor == floor).ToList();
                    return classrooms;
                }
            }            
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"GetClassrooms(Class) Exception caught:{ex.Message}");
                return null;
            }
        }
    }
}
