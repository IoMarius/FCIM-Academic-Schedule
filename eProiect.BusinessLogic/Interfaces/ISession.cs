
﻿using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule;

﻿using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Academic.DBModel;

using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace eProiect.BusinessLogic.Interfaces
{
     public interface ISession
     {
          ActionResponse UserLoginAction(ULoginData data);

          HttpCookie GenCookie(string loginCredential);

          ReducedUser GetUserByCookie(string cookieString);

          UserSchedule GetScheduleById(int id);

          /*GroupOfUsers GetAllUsers();
          ActionResponse AddNewUsers(NewUserData newUserData);
          ActionResponse EditUsers(User editedUserData);
          ActionResponse DeleteUsers(int Id);*/
          
          /*GroupOfDisciplines GetAllDisciplines();
          ActionResponse AddNewDisciplines(Discipline discipline);
          ActionResponse EditDisciplines(Discipline updateDisciline);
          ActionResponse DeleteDisciplines(int Id);*/
          
          /*AcademicGroupsList GetAllAcademicGroups();
          ActionResponse AddNewAcademicGroups(AcademicGroup academicGroup);
          ActionResponse EditAcademicGroups(AcademicGroup updateAcademicGroup);
          ActionResponse DeleteAcademicGroups(int Id);*/

          /*GroupOfCassRooms GetAllCassRooms();
          ActionResponse AddNewClassRooms(ClassRoom classRoom); 
          ActionResponse EditClassRooms(ClassRoom updateClassRoom);
          ActionResponse DeleteClassRooms(int Id);
*/

     }
}
