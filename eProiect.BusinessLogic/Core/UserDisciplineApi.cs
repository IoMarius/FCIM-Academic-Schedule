﻿using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using eProiect.Domain.Entities.Responce;
using MDD4All.SpecIF.DataModels.DiagramInterchange.BaseElements;
using System.Web.UI.WebControls.WebParts;
using System.Net.Http.Headers;
using System.Web.WebSockets;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Schedule;
using eProiect.Domain.Entities.User;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Dynamic;

namespace eProiect.BusinessLogic.Core
{
    public class UserDisciplineApi
    {
        internal List<UserDiscipline> GetAllUserDisciplineFromDb()
        {

            var userDisciplinesEntities = new List<UserDiscipline>();
            using (var db = new UserContext())
            {
                var userDisciplines = db.UserDisciplines
                    .Select(c => new
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        User = new
                        {
                            Id = c.User.Id,
                            Name = c.User.Name,
                            Surname = c.User.Surname
                        },
                        DisciplineId = c.DisciplineId,
                        Discipline = new
                        {
                            Id = c.Discipline.Id,
                            Name = c.Discipline.Name,

                        },
                        ClassTypeId = c.ClassTypeId,
                        Type = new
                        {
                            Id = c.Type.Id,
                            TypeName = c.Type.TypeName
                        }
                    })
                    .OrderBy(c => c.User.Surname)
                    .ToList();
                userDisciplinesEntities = userDisciplines.Select(c => new UserDiscipline
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    User = new User
                    {
                        Id = c.User.Id,
                        Name = c.User.Name,
                        Surname = c.User.Surname
                    },
                    DisciplineId = c.DisciplineId,
                    Discipline = new Discipline
                    {
                        Id = c.Discipline.Id,
                        Name = c.Discipline.Name,

                    },
                    ClassTypeId = c.ClassTypeId,
                    Type = new ClassType
                    {
                        Id = c.Type.Id,
                        TypeName = c.Type.TypeName
                    }
                }).ToList();
            };


            return userDisciplinesEntities;
        }

        internal ActionResponse DeleteUserDisciplineByIdFromDb(int idToDelete)
        {
            if (idToDelete <= 0)
                return new ActionResponse()
                {
                    Status = false,
                    ActionStatusMsg = "Id is wrong"
                };
            try
            {
                var userDisciplineToDelete = new UserDiscipline();
                using (var db = new UserContext())
                {
                    userDisciplineToDelete = db.UserDisciplines.FirstOrDefault(c => c.Id == idToDelete);
                    if (userDisciplineToDelete == null)
                        return new ActionResponse()
                        {
                            Status = false,
                            ActionStatusMsg = $"UserDiscipline with this id: {idToDelete} don't found"
                        };
                    db.UserDisciplines.Remove(userDisciplineToDelete);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return new ActionResponse()
                {
                    ActionStatusMsg = $"Error in connect to DB: error message: {ex.Message}",
                    Status = false
                };
            }
            return new ActionResponse()
            {
                ActionStatusMsg = $"UserDiscipline with id {idToDelete} was removed.",
                Status = true
            };
        }

        internal ActionResponse AddUserDisciplineToDb(UserDiscipline newUserDiscipline)
        {
            if (newUserDiscipline == null)
                return new ActionResponse()
                {
                    ActionStatusMsg = "Data for adding is null!!!",
                    Status = false
                };


            try
            {
                using (var db = new UserContext())
                {
                    var overlapWithUsetAndType = db.UserDisciplines
                        .Include(c => c.User)
                        .Include(c => c.Discipline)
                        .Include(c => c.Type)
                        .FirstOrDefault(c => c.UserId == newUserDiscipline.UserId &&
                                        c.ClassTypeId == newUserDiscipline.ClassTypeId &&
                                        c.DisciplineId == newUserDiscipline.DisciplineId);

                    if (overlapWithUsetAndType != null)
                        return new ActionResponse()
                        {
                            ActionStatusMsg = "Records with these values already exist",
                            Status = false
                        };

                    var user = db.Users
                        .FirstOrDefault(u => u.Id == newUserDiscipline.UserId);
                    var discilpline = db.Disciplines
                        .FirstOrDefault(c => c.Id == newUserDiscipline.DisciplineId);
                    var classType = db.ClassTypes
                        .FirstOrDefault(c => c.Id == newUserDiscipline.ClassTypeId);

                    if (classType == null || user == null || discilpline == null)
                        return new ActionResponse()
                        {
                            ActionStatusMsg = "Errrs to init UserDiscipline",
                            Status = false
                        };


                    var trueNewUserDiscipline = new UserDiscipline()
                    {
                        User = user,
                        Discipline = discilpline,
                        Type = classType
                    };

                    db.UserDisciplines.Add(trueNewUserDiscipline);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                return new ActionResponse()
                {
                    ActionStatusMsg = $"Error in connect to DB: error message: {ex.Message}",
                    Status = false
                };
            }


            return new ActionResponse()
            {
                ActionStatusMsg = "Discipline to user added succesed!!",
                Status = true
            };
        }

        internal List<UserDiscipline> GetUserDisciplinesByIdFromDb(int userId)
        {
            if (userId <= 0) return null;

            var userDisciplinesEntities = new List<UserDiscipline>();

            using (var db = new UserContext())
            {
                var userDisciplines = db.UserDisciplines
                    .Select(c => new
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        User = new
                        {
                            Id = c.User.Id,
                            Name = c.User.Name,
                            Surname = c.User.Surname
                        },
                        DisciplineId = c.DisciplineId,
                        Discipline = new
                        {
                            Id = c.Discipline.Id,
                            ShortName = c.Discipline.ShortName

                        },
                        ClassTypeId = c.ClassTypeId,
                        Type = new
                        {
                            Id = c.Type.Id,
                            TypeName = c.Type.TypeName
                        }
                    })
                    .Where(c => c.UserId == userId)
                    .ToList();
                userDisciplinesEntities = userDisciplines.Select(c => new UserDiscipline
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    User = new User
                    {
                        Id = c.User.Id,
                        Name = c.User.Name,
                        Surname = c.User.Surname
                    },
                    DisciplineId = c.DisciplineId,
                    Discipline = new Discipline
                    {
                        Id = c.Discipline.Id,
                        ShortName = c.Discipline.ShortName
                    },
                    ClassTypeId = c.ClassTypeId,
                    Type = new ClassType
                    {
                        Id = c.Type.Id,
                        TypeName = c.Type.TypeName
                    }
                }).ToList();
            }
            return userDisciplinesEntities;
        }

       

    }

}