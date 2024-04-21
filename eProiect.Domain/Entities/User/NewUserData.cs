using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.User
{
     public class NewUserData
     {
          public string Name { get; set; }
          public string Surname { get; set; }
          public UserRole Level { get; set; }
          public string Email { get; set; }
     }
}
