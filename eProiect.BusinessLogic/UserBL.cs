using eProiect.BusinessLogic.Core;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic
{
    public class UserBL : UserApi, IUser
    {
        public UserProfileData GetProfileData(int UserId)
        {
            return GetUserProfileById(UserId);
        }

        public ActionResponse EditUserPrifile(UserProfileData userProfile)
        {
            return EditUserPrifileAction(userProfile);
        }

        public ActionResponse ChangeUserPassword(UserPasswordChange userPasswordChange)
        {
            return ChangeUserPasswordAction(userPasswordChange);
        }
    }
}
