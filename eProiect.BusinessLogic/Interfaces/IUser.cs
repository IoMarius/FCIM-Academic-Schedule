using eProiect.Domain.Entities.Responce;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.Interfaces
{
    public interface IUser
    {
        UserProfileData GetProfileData(int UserId);
        ActionResponse EditUserPrifile(UserProfileData userProfile);
        ActionResponse ChangeUserPassword(UserPasswordChange userPasswordChange);
    }
}
