using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.DTOs.UserDtos
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="Passwords do not correspond")]
        public string ConfirmPassword { get; set; }
    }
}
