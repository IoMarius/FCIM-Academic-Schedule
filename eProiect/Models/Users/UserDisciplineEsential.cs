using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.Users
{
    public class UserDisciplineEsential
    {
        public int UserId { get; set; }
        public int DisciplineId { get; set; }
        public int DisciplineTypeId { get; set; }
    }
}