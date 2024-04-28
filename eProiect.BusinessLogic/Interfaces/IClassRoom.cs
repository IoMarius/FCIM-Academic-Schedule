using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.Interfaces
{
     public interface IClassRoom
     {
          GroupOfCassRooms GetAllCassRooms();
          ActionResponse AddNewClassRooms(ClassRoom classRoom);
          ActionResponse EditClassRooms(ClassRoom updateClassRoom);
          ActionResponse DeleteClassRooms(int Id);

     }
}
