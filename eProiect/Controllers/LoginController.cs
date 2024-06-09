using eProiect.BusinessLogic.Interfaces;
using eProiect.BusinessLogic;
using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eProiect.Models.Users;
using eProiect.Domain.Entities.Responce;
using eProiect.Atributes;
using AutoMapper.Features;

namespace eProiect.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISession _session;

        // GET: Register
        public LoginController()
        {
            var bl = new BuissinesLogic();
            _session = bl.GetSessionBL();
        }

        // GET : Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin data)
        {
            if (ModelState.IsValid)
            {

                ULoginData uData = new ULoginData
                {
                    Credential = data.Credential,
                    Password = data.Password,
                    LoginIp = Request.UserHostAddress,
                    LoginDateTime = DateTime.Now
                };

                ActionResponse resp = _session.UserLoginAction(uData);
                ViewBag.LogSuccess = resp.Status;
                if (resp.Status)
                {
                    //ADD COOKIE
                    //coogie
                    HttpCookie cookie = _session.GenCookie(uData.Credential);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    return RedirectToAction("Schedule", "Home");
                }
                else
                {
                    ModelState.AddModelError("", resp.ActionStatusMsg);
                    return View();
                }

            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginError()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginError(string userEmail)
        {

            var response = _session.SendResetPasswordCodeByEmail(userEmail);

            return Json(new { success = response.Status, message = response.ActionStatusMsg });

        }

        [HttpPost]
        public ActionResult ResetPassword(ResetUserPasswordViewModel resetUserPasswordViewModel)
        {
            if (resetUserPasswordViewModel == null)
                return Json(new { success = false, message = "Error" });

            if(resetUserPasswordViewModel.Password != resetUserPasswordViewModel.ConfirmePassword)
                return Json(new { success = false, message = "Error" });

            var resetData = new ResetUserPasswordData
            {
                ResetCode = resetUserPasswordViewModel.ResetCode,
                Email = resetUserPasswordViewModel.Email,
                Password = resetUserPasswordViewModel.Password
            };

            var response = _session.SetNewPasswordByResetCode(resetData);

            return Json(new { success = response.Status, message = response.ActionStatusMsg });
        }

    }
}