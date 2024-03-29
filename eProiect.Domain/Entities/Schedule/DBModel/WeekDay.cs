using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Schedule.DBModel
{
    public class WeekDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "Out of size length limits [3-8]")]
        public string Name { get; set; }

        [Required]
        [StringLength(2)] //lu ma mi jo vi sa
        public string ShortName { get; set; }


        //NOT SURE
        public ICollection<Class> Classes { get; set; }
    }
}
