using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.Users
{
    public class UserEsentialData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }

        public UserRole Level { get; set; }
    }
}