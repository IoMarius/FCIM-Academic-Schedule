using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.Domain.Entities.Schedule;

namespace eProiect.BusinessLogic.Interfaces
{
     public interface IClassRoom
     {
          GroupOfCassRooms GetAllCassRooms();
          ActionResponse AddNewClassRooms(ClassRoom classRoom);
          ActionResponse EditClassRooms(ClassRoom updateClassRoom);
          ActionResponse DeleteClassRooms(int Id);

        /// <summary>
        /// Queries the database table "Classrooms" for free classrooms at
        /// given parameters(weekday, time, span, frequency);
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<ClassRoom> GetFreeClassroomsByFloorAndTime(FreeClassroomsRequest data);

        List<ClassRoom> GetClassRoomsByFloor(int floor);

    }
}
