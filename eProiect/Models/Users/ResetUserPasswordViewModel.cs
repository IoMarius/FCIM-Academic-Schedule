using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.Users
{
    public class ResetUserPasswordViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmePassword { get; set; }
        public int ResetCode {  get; set; }
    }
}