using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;

using eProiect.Domain.Entities.Academic;

using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eProiect.BusinessLogic
{
     public class SessionBL : UserApi, ISession
     {
        public ActionResponse UserLoginAction(ULoginData data)
        {
            return RLoginUpService(data);
        }

        public HttpCookie GenCookie(string loginCredential)
        {
            return Cookie(loginCredential);
        }

        public ReducedUser GetUserByCookie(string cookieString){
            return UserByCookie(cookieString);
        }


        public UserSchedule GetScheduleById(int id)
        {
            return GetUserScheduleById(id);
        }

          public GroupOfUsers GetAllUsers()
          {
               return GetAllUser();
          }
          public GroupOfDisciplines GetAllDisciplines()
          {
               return GetAllDiscipline();
          }
          public AcademicGroupsList GetAllAcademicGroups()
          {
               return GetAllAcademicGroup();
          }
          public GroupOfCassRooms GetAllCassRooms()
          {
               return GetAllClassRoom();
          }

          public ActionResponse AddNewUsers(NewUserData newUserData )
          {
               return AddNewUser(newUserData);
          }
          public ActionResponse AddNewAcademicGroups(AcademicGroup academicGroup)
          {
               return AddNewAcademicGroup(academicGroup);
          }
          public ActionResponse AddNewDisciplines( Discipline discipline )
          {
               return AddNewDiscipline(discipline);
          }
          public ActionResponse AddNewClassRooms(ClassRoom classRoom)
          {
               return AddNewClassRoom(classRoom);
          }
          
          public ActionResponse DeleteUsers(int Id)
          {
               return DeleteUser(Id);
          }
          public ActionResponse DeleteDisciplines(int Id)
          {
               return DeleteDiscipline(Id);
          }
          public ActionResponse DeleteAcademicGroups(int Id)
          {
               return DeleteAcademicGroup(Id);
          }
          public ActionResponse DeleteClassRooms(int Id)
          {
               return DeleteClassRoom(Id);
          }


     }


}
