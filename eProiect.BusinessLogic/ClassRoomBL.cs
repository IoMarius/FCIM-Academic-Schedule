using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;

namespace eProiect.BusinessLogic
{
     public class ClassRoomBL :ClassRoomApi ,IClassRoom
     {
          public GroupOfCassRooms GetAllCassRooms()
          {
               return GetAllClassRoom();
          }
          public ActionResponse AddNewClassRooms(ClassRoom classRoom)
          {
               return AddNewClassRoom(classRoom);
          }
          public ActionResponse EditClassRooms(ClassRoom updatedClassRoomData)
          {
               return EditClassRoom(updatedClassRoomData);
          }
          public ActionResponse DeleteClassRooms(int Id)
          {
               return DeleteClassRoom(Id);
          }

     }
}
