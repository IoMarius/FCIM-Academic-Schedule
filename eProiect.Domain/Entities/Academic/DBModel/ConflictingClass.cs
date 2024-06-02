using eProiect.Domain.Entities.Schedule.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.Academic.DBModel
{
    public class ConflictingClass
    {
        [Key]
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        /*public int MainClassId { get; set; }*/
        public Class MainClass { get; set; }

        [Required]
        /*public int ConflictingWithId { get; set; }*/
        public Class ConflictingWith { get; set; }
    }
}
