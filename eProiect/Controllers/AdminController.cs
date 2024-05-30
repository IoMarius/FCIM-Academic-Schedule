using eProiect.BusinessLogic.Interfaces;
using eProiect.Extensions;
using eProiect.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eProiect.Models;
using eProiect.Models.ViewModels;
using eProiect.Atributes;
using eProiect.Domain.Entities.User;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.User.DBModel;
using eProiect.Domain.Entities.Academic.DBModel;
using AutoMapper.Configuration.Annotations;
using System.Runtime.CompilerServices;

namespace eProiect.Controllers
{
    public class AdminController : BaseController
    {

        private readonly IAdmin _admin;
        private readonly IDiscipline _discipline;
        private readonly IUserDiscipline _userDiscipline;
        private readonly IAcademicGroup _academicGroup;
        private readonly IClassRoom _classRoom;

        public AdminController()
        {
            var bl = new BusinessLogic.BuissinesLogic();
            _admin = bl.GetAdminBL();
            _academicGroup = bl.GetAcademicGroupBL();
            _classRoom = bl.GetClassRoomBL();
            _discipline = bl.GetDisciplineBL();
            _userDiscipline = bl.GetUserDisciplineBL();
        }

        /// <summary>
        /// Retrieves a list of disciplines for administrative purposes.
        /// </summary>
        /// <returns>An ActionResult representing the Disciplines view populated with discipline data.</returns>
        [UserMode(UserRole.admin)]
        public ActionResult Users()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData
            {
                UDataList = new List<UserEsentialData>()
            };


            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            var users = _admin.GetAllUsers();


            foreach (var user in users.Users)
            {
                UserEsentialData eUser = new UserEsentialData
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Credentials.Email,
                    Level = user.Level
                };
                viewData.UDataList.Add(eUser);
            }





            return View(viewData);
        }


        [UserMode(UserRole.admin)]
        public ActionResult AddUser()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData();

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            return View(viewData);
        }

        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult AddUser(UserEsentialData userEsentialData)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                var newUserData = new NewUserData
                {
                    Name = userEsentialData.Name,
                    Surname = userEsentialData.Surname,
                    Level = userEsentialData.Level,
                    Email = userEsentialData.Email
                };

                var response = _admin.AddNewUsers(newUserData);

                return Json(new { success = response.Status, message = response.ActionStatusMsg });
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }

        /// <summary>
        /// Edits user data based on the provided UserEsentialData object.
        /// </summary>
        /// <param name="userData">The UserEsentialData object containing the updated user data.</param>
        /// <returns>A JSON object indicating the success or failure of the operation along with a message.</returns>
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult EditUserData(UserEsentialData userData)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Construct edited user data
                    var editedUserData = new User
                    {
                        Id = userData.Id,
                        Name = userData.Name,
                        Surname = userData.Surname,
                        Level = userData.Level,
                        Credentials = new UserCredential
                        {
                            Id = userData.Id,
                            Email = userData.Email
                        }
                    };

                    // Call session method to edit user
                    var response = _admin.EditUsers(editedUserData);

                    // Return success or failure message
                    return Json(new { success = response.Status, message = response.ActionStatusMsg });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while editing user data: " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }

        /// <summary>
        /// Deletes a user based on the provided user ID.
        /// </summary>
        /// <param name="userIdForDeleted">The ID of the user to be deleted.</param>
        /// <returns>A JSON object indicating the success or failure of the operation along with a message.</returns>
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult DeleteUser(int userIdForDeleted)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Call session method to edit user
                    var response = _admin.DeleteUsers(userIdForDeleted);
                    // Return success or failure message
                    return Json(new { success = response.Status, message = response.ActionStatusMsg });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while delete user " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }






        /// <summary>
        /// Retrieves a list of disciplines for administrative purposes.
        /// </summary>
        /// <returns>An ActionResult representing the Disciplines view populated with discipline data.</returns>
        [UserMode(UserRole.admin)]
        public ActionResult Disciplines()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData();

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            viewData.DisciplinesDataList = _discipline.GetAllDisciplines();

            return View(viewData);
        }

        [UserMode(UserRole.admin)]
        public ActionResult AddDiscipline()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData();

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            return View(viewData);
        }

        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult AddDiscipline(Discipline disciplinesData)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                var newDisciplinesData = new Discipline
                {
                    Name = disciplinesData.Name,
                    ShortName = disciplinesData.ShortName
                };

                var response = _discipline.AddNewDisciplines(newDisciplinesData);

                return Json(new { success = response.Status, message = response.ActionStatusMsg });
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }

        /// <summary>
        /// Edits a discipline's data based on the provided Discipline object.
        /// </summary>
        /// <param name="discipline">The Discipline object containing the updated discipline data.</param>
        /// <returns>A JSON object indicating the success or failure of the operation along with a message.</returns>
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult EditDisciplineData(Discipline discipline)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Construct edited user data
                    var editedDisciplineData = new Discipline
                    {
                        Id = discipline.Id,
                        Name = discipline.Name,
                        ShortName = discipline.ShortName
                    };

                    // Call session method to edit user
                    var response = _discipline.EditDisciplines(editedDisciplineData);

                    // Return success or failure message
                    return Json(new { success = response.Status, message = response.ActionStatusMsg });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while editing disciplines data: " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }

        /// <summary>
        /// Deletes a discipline based on the provided discipline ID.
        /// </summary>
        /// <param name="disciplineIdForDeleted">The ID of the discipline to be deleted.</param>
        /// <returns>A JSON object indicating the success or failure of the operation along with a message.</returns>
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult DeleteDiscipline(int disciplineIdForDeleted)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Call session method to edit user
                    var response = _discipline.DeleteDisciplines(disciplineIdForDeleted);
                    // Return success or failure message
                    return Json(new { success = response.Status, message = response.ActionStatusMsg });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while delete discipline " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }



        /// <summary>
        /// Retrieves a list of academic groups for administrative purposes.
        /// </summary>
        /// <returns>An ActionResult representing the AcademicGroups view populated with academic group data.</returns>
        [UserMode(UserRole.admin)]
        public ActionResult AcademicGroups()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData();

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            viewData.AcademicGroupsDataList = _academicGroup.GetAllAcademicGroups();

            return View(viewData);
        }

        [UserMode(UserRole.admin)]
        public ActionResult AddAcademicGroup()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData();

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            return View(viewData);
        }

        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult AddAcademicGroup(AcademicGroup academicGroupData)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                var newAcademicGroupData = new AcademicGroup
                {
                    Name = academicGroupData.Name,
                    Year = academicGroupData.Year
                };

                var response = _academicGroup.AddNewAcademicGroups(newAcademicGroupData);

                return Json(new { success = response.Status, message = response.ActionStatusMsg });
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }

        /// <summary>
        /// Edits an academic group's data based on the provided AcademicGroup object.
        /// </summary>
        /// <param name="academicGroup">The AcademicGroup object containing the updated academic group data.</param>
        /// <returns>A JSON object indicating the success or failure of the operation along with a message.</returns>
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult EditAcademicGroupData(AcademicGroup academicGroup)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Construct edited user data
                    var editedAcademicGroup = new AcademicGroup
                    {
                        Id = academicGroup.Id,
                        Name = academicGroup.Name,
                        Year = academicGroup.Year
                    };

                    // Call session method to edit user
                    var response = _academicGroup.EditAcademicGroups(editedAcademicGroup);

                    // Return success or failure message
                    return Json(new { success = response.Status, message = response.ActionStatusMsg });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while editing group data: " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }

        /// <summary>
        /// Deletes an academic group based on the provided academic group ID.
        /// </summary>
        /// <param name="academicGroupIdForDeleted">The ID of the academic group to be deleted.</param>
        /// <returns>A JSON object indicating the success or failure of the operation along with a message.</returns>
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult DeleteAcademicGroup(int academicGroupIdForDeleted)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Call session method to edit user
                    var response = _academicGroup.DeleteAcademicGroups(academicGroupIdForDeleted);
                    // Return success or failure message
                    return Json(new { success = response.Status, message = response.ActionStatusMsg });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while delete academic group " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }



        /// <summary>
        /// Retrieves a list of classrooms for administrative purposes.
        /// </summary>
        /// <returns>An ActionResult representing the ClassRooms view populated with classroom data.</returns>
        [UserMode(UserRole.admin)]
        public ActionResult ClassRooms()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData();

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            viewData.CassRoomsDataList = _classRoom.GetAllCassRooms();

            return View(viewData);
        }

        [UserMode(UserRole.admin)]
        public ActionResult AddClassRoom()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData();

            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            return View(viewData);
        }

        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult AddClassRoom(ClassRoom classRoomData)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                var newClassRoomDataData = new ClassRoom
                {
                    ClassroomName = classRoomData.ClassroomName,
                    Floor = classRoomData.Floor
                };

                var response = _classRoom.AddNewClassRooms(newClassRoomDataData);

                return Json(new { success = response.Status, message = response.ActionStatusMsg });
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }

        /// <summary>
        /// Edits a classroom's data based on the provided ClassRoom object.
        /// </summary>
        /// <param name="classRoom">The ClassRoom object containing the updated classroom data.</param>
        /// <returns>A JSON object indicating the success or failure of the operation along with a message.</returns>
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult EditClassRoomData(ClassRoom classRoom)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Construct edited user data
                    var editedClassRoom = new ClassRoom
                    {
                        Id = classRoom.Id,
                        ClassroomName = classRoom.ClassroomName,
                        Floor = classRoom.Floor
                    };

                    // Call session method to edit user
                    var response = _classRoom.EditClassRooms(editedClassRoom);

                    // Return success or failure message
                    return Json(new { success = response.Status, message = response.ActionStatusMsg });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while editing classRoom data: " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }

        /// <summary>
        /// Deletes a classroom based on the provided classroom ID.
        /// </summary>
        /// <param name="classRoomIdForDeleted">The ID of the classroom to be deleted.</param>
        /// <returns>A JSON object indicating the success or failure of the operation along with a message.</returns>
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult DeleteClassRoom(int classRoomIdForDeleted)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Call session method to edit user
                    var response = _classRoom.DeleteClassRooms(classRoomIdForDeleted);
                    // Return success or failure message
                    return Json(new { success = response.Status, message = response.ActionStatusMsg });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while delete academic group " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }






        [UserMode(UserRole.admin)]
        public ActionResult UserDiscipline()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            GeneralViewData viewData = new GeneralViewData();
            var loggedInUser = System.Web.HttpContext.Current.GetMySessionObject();
            UserEsentialData UData = new UserEsentialData
            {
                Name = loggedInUser.Name,
                Surname = loggedInUser.Surname,
                CreatedDate = loggedInUser.CreatedDate,
                Level = loggedInUser.Level
            };
            viewData.UData = UData;

            return View(viewData);

        }

        [HttpGet]
        [UserMode(UserRole.admin)]
        public ActionResult GetAllUsersDisciplines()
        {
            /*SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }*/

            return Json
                (
                    _userDiscipline.GetAllUserDiscipline(),
                    JsonRequestBehavior.AllowGet
                );
        }

        [HttpGet]
        [Route("Admin/GetDiscilineByUserId/{userId}")]
        [UserMode(UserRole.admin)]
        public ActionResult GetDiscilineByUserId(int userId)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");

            }

            return Json
                (
                    _userDiscipline.GetUserDisciplineById(userId),
                    JsonRequestBehavior.AllowGet
                );
        }

        [HttpGet]
        [UserMode(UserRole.admin)]
        public ActionResult GetAllDiscipline()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");

            }
            var listOfDisciplines = _discipline.GetAllDisciplines();

            return Json
                (
                    listOfDisciplines.Disciplines,
                    JsonRequestBehavior.AllowGet
                );

        }
        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult AddUserDiscipline(List<UserDisciplineEsential> userDisciplineEsentials)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid && userDisciplineEsentials != null)
            {
                try
                {
                    // Call session method to edit use
                    foreach (var userDiscipline in userDisciplineEsentials)
                    {
                        var trueUserDiscipline = new UserDiscipline
                        {
                            UserId = userDiscipline.UserId,
                            DisciplineId = userDiscipline.DisciplineId,
                            ClassTypeId = userDiscipline.DisciplineTypeId
                        };

                        var response = _userDiscipline.AddUserDiscipline(trueUserDiscipline);
                        if (!response.Status)
                            return Json(new { success = response.Status, message = response.ActionStatusMsg });
                    }
                    return Json(new { success = true, message = " UserDiscipline was added" });
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    return Json(new { success = false, message = "An error occurred while added user discipline " + ex.Message });
                }
            }
            else
            {
                // Return validation error message
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }

        }

        [HttpPost]
        [UserMode(UserRole.admin)]
        public ActionResult DeleteUserDsiciplineByUserDisciplineId(int userDisciplineId)
        {
            SessionStatus();

            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = _userDiscipline.DeleteUserDisciplineById(userDisciplineId);
                    if (!response.Status)
                    {
                        return Json(new { success = response.Status, message = response.ActionStatusMsg });
                    }
                    else
                    {
                        return Json(new { success = true, message = "User discipline deleted successfully" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while deleting user discipline: " + ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid data. Please check the provided information." });
            }
        }


        [HttpGet]
        [UserMode(UserRole.admin)]
        public ActionResult GetAllUserEsentialsData()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }
            return Json
                (
                    _admin.GetAllUserRedusedUserData(),
                    JsonRequestBehavior.AllowGet
                ); ;
        }
    }
}
