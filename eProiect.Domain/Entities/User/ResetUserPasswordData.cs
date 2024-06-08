using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.User
{
    public class ResetUserPasswordData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int  ResetCode { get; set; }
    }
}
