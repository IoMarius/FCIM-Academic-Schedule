using eProiect.Domain.Entities.Schedule.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Academic.DBModel
{
    public class ClassRoom
    {
        [Key]
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(10, MinimumLength=3, ErrorMessage ="Incompliant classroom name size [3-10].")]
        public string ClassroomName { get; set; }

         [Required]
        public int Floor {  get; set; }
    }
}

