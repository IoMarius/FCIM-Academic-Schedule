using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;

using eProiect.BusinessLogic.Migrations;

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

         /* public GroupOfUsers GetAllUsers()
          {
               return GetAllUser();
          }
          public ActionResponse AddNewUsers(NewUserData newUserData )
          {
               return AddNewUser(newUserData);
          }
          public ActionResponse EditUsers(User editedUserData)
          {
               return EditUser(editedUserData);
          }
          public ActionResponse DeleteUsers(int Id)
          {
               return DeleteUser(Id);
          }*/
         

       /*   public AcademicGroupsList GetAllAcademicGroups()
          {
               return GetAllAcademicGroup();
          }
          public ActionResponse AddNewAcademicGroups(AcademicGroup academicGroup)
          {
               return AddNewAcademicGroup(academicGroup);
          }
          public ActionResponse EditAcademicGroups(AcademicGroup updateacademicGroup)
          {
               return EditAcademicGroup( updateAcademicGroupData);
          }
          public ActionResponse DeleteAcademicGroups(int Id)
          {
               return DeleteAcademicGroup(Id);
          }
        */
          /*public GroupOfCassRooms GetAllCassRooms()
          {
               return GetAllClassRoom();
          }
          public ActionResponse AddNewClassRooms(ClassRoom classRoom)
          {
               return AddNewClassRoom(classRoom);
          }
          public ActionResponse EditClassRooms(ClassRoom updatedClassRoomData)
          {
               return EditClassRoom( updatedClassRoomData);
          }
          public ActionResponse DeleteClassRooms(int Id)
          {
               return DeleteClassRoom(Id);
          }
*/
         /* public GroupOfDisciplines GetAllDisciplines()
          {
               return GetAllDiscipline();
          }
          public ActionResponse AddNewDisciplines( Discipline discipline )
          {
               return AddNewDiscipline(discipline);
          }
          public ActionResponse EditDisciplines(Discipline updateDiscipline)
          {
               return new EditDiscipline( updateDiscipline);
          }
          public ActionResponse DeleteDisciplines(int Id)
          {
               return DeleteDiscipline(Id);
          }*/
     }


}
