using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProiect.Domain.Entities.User
{
    public class ReducedUser
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime LastLogin { get; set; }

		public string LastIp { get; set; }

		public UserRole Level { get; set; }
	}
}
