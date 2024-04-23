using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.Schedule.DBModel;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
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

          List<AcademicGroup> GetAcadGroupsList();

          List<AcademicGroup> GetAcadGroupsList(int year);

          List<UserDiscipline> GetDisciplinesById(int id);

          List<ClassRoom> GetFreeClassroomsByFloor(int floor);
     }
}
