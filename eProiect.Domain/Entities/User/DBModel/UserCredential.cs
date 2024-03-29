using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.User.DBModel
{
    public class UserCredential
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password does not comply to size limits [8-20].")]
        public string Password { get; set; }
    }
}
