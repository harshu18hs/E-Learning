using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ObjectModel
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        [Required]
        public string Role { get; set; }
    }
}
