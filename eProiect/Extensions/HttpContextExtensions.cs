using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Extensions
{
    public static class HttpContextExtensions
    {
        public static ReducedUser GetMySessionObject(this HttpContext current)
        {
            return (ReducedUser)current?.Session["__SessionObject"];
        }

        public static void SetMySessionObject(this HttpContext current, ReducedUser profile)
        {
            current.Session.Add("__SessionObject", profile);
        }
    }
}