using eProiect.BusinessLogic;
using eProiect.BusinessLogic.Interfaces;
using eProiect.Domain.Entities.User;
using eProiect.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eProiect.Atributes
{
     public class UserModeAttribute : ActionFilterAttribute
     {
          private readonly ISession _session;
          private UserRole _userRole1 , _userRole2;
               public UserModeAttribute(UserRole userRole)
               {
                    var bl = new BuissinesLogic();
                    _session = bl.GetSessionBL();
                    _userRole1 = userRole;
                    _userRole2 = UserRole.guest;  
               }
               public UserModeAttribute(UserRole userRole1, UserRole userRole2)
               {
                    var bl = new BuissinesLogic();
                    _session = bl.GetSessionBL();
                    _userRole1 = userRole1;
                    _userRole2 = userRole2;
               }


          public override void OnActionExecuting(ActionExecutingContext filterContext)
               {
                    var apiCookie = HttpContext.Current.Request.Cookies["X-KEY"];
                    if (apiCookie != null)
                    {
                         var profile = _session.GetUserByCookie(apiCookie.Value);
                    if (_userRole2 != UserRole.guest)
                    {

                         if (profile != null && (profile.Level == _userRole1 || profile.Level == _userRole2))
                         {
                              HttpContext.Current.SetMySessionObject(profile);
                         }
                         else
                         {
                              filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Login" }));
                         }

                    } else if (profile != null && profile.Level == _userRole1)
                         {
                         HttpContext.Current.SetMySessionObject(profile);
                         } else
                              {
                                   filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Login" }));
                              }
                    }
               }
          
     }
}