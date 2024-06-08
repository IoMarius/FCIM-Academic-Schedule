using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.User.DBModel
{
    public class UserResetPassword
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(90)]
        public string Email { get; set; }

        [Required]
        public DateTime ExpireTime { get; set; }

        [Required]
        public int ResetCode { get; set; }

    }
}
