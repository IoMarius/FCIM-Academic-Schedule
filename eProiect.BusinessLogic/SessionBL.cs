﻿using eProiect.BusinessLogic.Core;
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

        public ReducedUser GetUserByCookie(string cookieString)
        {
            return UserByCookie(cookieString);
        }

        public ActionResponse SendResetPasswordCodeByEmail(string loginEmail)
        {
            return SendUserResetPasswordCodeByEmail(loginEmail);
        }

        public ActionResponse SetNewPasswordByResetCode(ResetUserPasswordData resetUserPasswordData)
        {
            return SetNewUserPasswordActionByResetCode(resetUserPasswordData);
        }
    }


}
