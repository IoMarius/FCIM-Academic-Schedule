using eProiect.BusinessLogic.DBModel;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Responce;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.BusinessLogic.Core
{
     public class DisciplineApi
     {
          internal GroupOfDisciplines GetAllDiscipline()
          {
               var allDisciplines = new GroupOfDisciplines();

               using (var db = new UserContext())
               {
                    allDisciplines.Disciplines = db.Disciplines.ToList();
               }
               return allDisciplines;
          }
          internal ActionResponse AddNewDiscipline(Discipline discipline)
          {
               var newDiscipline = new Discipline
               {
                    Name = discipline.Name,
                    ShortName = discipline.ShortName
               };

               using (var db = new UserContext())
               {

                    try
                    {
                         db.Disciplines.Add(newDiscipline);
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
                    return new ActionResponse { Status = true };
               }
          }
          internal ActionResponse EditDiscipline(Discipline updatedDisciplineData)
          {
               try
               {
                    using (var db = new UserContext())
                    {
                         var discipline = db.Disciplines.FirstOrDefault(d => d.Id == updatedDisciplineData.Id);
                         if (discipline == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Discipline not found or invalid ID",
                                   Status = false
                              };
                         }

                         // Update discipline properties
                         discipline.Name = updatedDisciplineData.Name;
                         discipline.ShortName = updatedDisciplineData.ShortName;

                         db.SaveChanges();
                    }

                    return new ActionResponse
                    {
                         ActionStatusMsg = "Discipline data updated successfully",
                         Status = true
                    };
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating discipline data: {ex.Message}",
                         Status = false
                    };
               }
          }
          internal ActionResponse DeleteDiscipline(int Id)
          {
               if (Id < 0 && Id != 0)
                    return new ActionResponse
                    {
                         ActionStatusMsg = "Invalid ID",
                         Status = false
                    };
               try
               {
                    Discipline _discipline;
                    using (var db = new UserContext())
                    {
                         _discipline = db.Disciplines.FirstOrDefault(d => d.Id == Id);
                         if (_discipline == null)
                         {
                              return new ActionResponse
                              {
                                   ActionStatusMsg = "Discipline not found or invalid ID",
                                   Status = false
                              };
                         }

                         db.Disciplines.Remove(_discipline);
                         db.SaveChanges();
                    }
               }
               catch (Exception ex)
               {
                    return new ActionResponse
                    {
                         ActionStatusMsg = $"An error occurred while updating discipline data: {ex.Message}",
                         Status = false
                    };
               }
               return new ActionResponse
               {
                    ActionStatusMsg = "Discipline data updated successfully",
                    Status = true
               };

          }
     }
}
