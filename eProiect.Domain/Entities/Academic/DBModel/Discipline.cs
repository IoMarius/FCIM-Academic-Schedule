using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Academic.DBModel
{
    public class Discipline
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength =2, ErrorMessage ="Discipline name incompliant with [1-50] size")]
        public string Name { get; set; }
 
        [Required]
        [StringLength(15, MinimumLength =1)]
        public string ShortName { get; set; }
    }
}
