using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Academic.DBModel
{
     public class AcademicGroup
     {
          [Key]
          [Index(IsUnique = true)]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }

          [Required]

          [StringLength(10, MinimumLength =6)]
          public string Name { get; set; }

          [Required]
          public int Year { get; set; }
     }

}
