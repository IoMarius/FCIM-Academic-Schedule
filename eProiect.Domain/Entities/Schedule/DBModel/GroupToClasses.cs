using eProiect.Domain.Entities.Academic.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Schedule.DBModel
{
     public class GroupToClasses
     {
          [Required]
          public int ClassId { get; set; }
          public Class Class { get; set; }

          [Required]
          public int AcademicGroupId { get; set; }
          public AcademicGroup AcademicGroup { get; set; }
          
     }
}
