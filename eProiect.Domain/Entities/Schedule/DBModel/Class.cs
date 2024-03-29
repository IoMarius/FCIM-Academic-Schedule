using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Academic.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Schedule.DBModel
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Discipline Discipline { get; set; }

        [Required]
        public ClassType Type {get; set; }

        [Required]
        public ClassRoom Classoom { get; set; }

        [Required]
        public WeekDay WeekDay { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [DefaultValue(ClassFrequency.Weekly)]
        public ClassFrequency Frequency { get; set; }
    }
}
