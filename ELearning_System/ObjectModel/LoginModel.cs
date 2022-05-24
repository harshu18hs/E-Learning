using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace ObjectModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
