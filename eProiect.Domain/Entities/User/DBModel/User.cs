using eProiect.Domain.Entities.User.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.User
{
    //User db table 
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name="Name")]
        [StringLength(30, MinimumLength =3, ErrorMessage ="Short name")]
        public string Name { get; set; }
        
        [Required]
        [Display(Name="Surname")]
        [StringLength(30, MinimumLength =3, ErrorMessage ="Short surname")]
        public string Surname { get; set; }
       
        [Required]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastLogin { get; set; }

        [StringLength(30)]
        public string LastIp { get; set; }

        [DefaultValue(UserRole.guest)]
        public UserRole Level { get; set; }
        //if level != guest treb sa aiba ce obiect/obiecte duce si ce tip/tipuri


        [Required]
        public UserCredential Credentials { get; set; }        
    }
}
