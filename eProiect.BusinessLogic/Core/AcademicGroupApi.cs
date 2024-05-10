using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProiect.BusinessLogic.Interfaces;

namespace eProiect.BusinessLogic.Core
{
     public class AcademicGroupApi
     {
          internal AcademicGroupsList GetAllAcademicGroup()
          {

               var allAcademicGroups = new AcademicGroupsList();

               using (var db = new UserContext())
               {
                    
                    allAcademicGroups.AcademicGroups = db.AcademicGroups.ToList();
               }
               return allAcademicGroups;
          }
          internal ActionResponse AddNewAcademicGroup(AcademicGroup academicGroup)
          {
               var newAcademicGroup = new AcademicGroup
               {
                    Name = academicGroup.Name,
                    Year = academicGroup.Year
               };

               using (var db = new UserContext())
               {

                    try
                    {
                         db.AcademicGroups.Add(newAcademicGroup);
                         db.SaveChanges();

                    }
                    catch (DbUpdateException ex)
                    {
                         if (ex.InnerException != null)
                         {

                              // Check if inner exception is SqlException
                              if (ex.InnerException is SqlException sqlEx)
                              {
                                   // Handle SqlException
                                   if (sqlEx.Number == 2601) // SQL Server error number for unique constraint violation
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Duplicate key error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                                   else
                                   {
                                        return new ActionResponse
                                        {
                                             ActionStatusMsg = "Other SQL Server error occurred: " + sqlEx.Message,
                                             Status = false
                                        };
                                   }
                              }
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Inner Exception: " + ex.InnerException.Message,
                                   Status = false
                              };
                         }
                         return new ActionResponse
                         {
                              ActionStatusMsg = "DbUpdateException occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    catch (Exception ex)
                    {
                         // Handle other exceptions
                         return new ActionResponse
                         {
                              ActionStatusMsg = "An error occurred: " + ex.Message,
                              Status = false
                         };
                    }
                    return new ActionResponse 
                    {
                         ActionStatusMsg = "Academic Group was successfully added.",
                         Status = true
                    };
               }
          }
          internal ActionResponse EditAcademicGroup(AcademicGroup newAcademicGroupData)
          {
               if (newAcademicGroupData == null)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Null parametre newAcademicGroupData",
                         Status = false
                    };
               }
               var validate = new EmailAddressAttribute();

               try
               {
                    using (var db = new UserContext())
                    {
                         var _academicGroup = db.AcademicGroups.FirstOrDefault(u => u.Id == newAcademicGroupData.Id);
                         if (_academicGroup == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Academic group not found or invalid ID",
                                   Status = false
                              };
                         }

                         _academicGroup.Name = newAcademicGroupData.Name;
                         _academicGroup.Year = newAcademicGroupData.Year;

                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating user data: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "User data updated successfully",
                    Status = true
               };


          }
          internal ActionResponse DeleteAcademicGroup(int Id)
          {
               if (Id < 0 && Id != 0)
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Invalid ID",
                         Status = false
                    };
               try
               {
                    AcademicGroup _academicGroup;
                    using (var db = new UserContext())
                    {
                         _academicGroup = db.AcademicGroups.FirstOrDefault(g => g.Id == Id);
                         if (_academicGroup == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Academic group not found or invalid ID",
                                   Status = false
                              };
                         }

                         db.AcademicGroups.Remove(_academicGroup);
                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating academic group data: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "Academic group data updated successfully",
                    Status = true
               };
          }
     }
}
