using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Schedule
{
    //modify to class for db table
    public class ClassType
    {
        [Key]
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]  
        [StringLength(25, MinimumLength = 2)]
        public string TypeName { get; set; }
    }
}
