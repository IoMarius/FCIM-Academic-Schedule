using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.User
{
    public class SubscribeUserRequest
    {
        public string GuestEmail { get; set; }
        public int AcademicGroupId { get; set; }
    }
}
