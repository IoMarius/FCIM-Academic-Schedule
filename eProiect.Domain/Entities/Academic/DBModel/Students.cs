using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Academic.DBModel
{
     public class Students
     {
          [Key]
          [Index(IsUnique = true)]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }

          [Required]
          public int AcademicGroupId { get; set; }
          public AcademicGroup AcademicGroup { get; set; }

          [Required]
          [Display(Name = "Email Address")]
          [StringLength(90)]
          public string Email { get; set; }
     }
}
