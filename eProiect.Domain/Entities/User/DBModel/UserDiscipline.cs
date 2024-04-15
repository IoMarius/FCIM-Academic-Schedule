using eProiect.Domain.Entities.Academic.DBModel;
using eProiect.Domain.Entities.Schedule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.User.DBModel
{
    public class UserDiscipline
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }

        [Required]
        public int ClassTypeId { get; set; }
        public ClassType Type { get; set; }
    }
}
