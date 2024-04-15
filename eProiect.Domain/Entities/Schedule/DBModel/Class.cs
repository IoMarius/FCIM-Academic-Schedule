using eProiect.Domain.Entities.Academic;
using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.User.DBModel;
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
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserDisciplineId {  get; set; }
        public UserDiscipline UserDiscipline { get; set; }

        [Required]
        public int AcademicGroupId { get; set; }
        public AcademicGroup AcademicGroup { get; set; }

        [Required]
        public int ClassRoomId {  get; set; }
        public ClassRoom ClassRoom { get; set; }


        [Required]
        public int WeekDayId { get; set; }
        public WeekDay WeekDay { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [DefaultValue(ClassFrequency.WEEKLY)]
        public ClassFrequency Frequency { get; set; }
    }
}
