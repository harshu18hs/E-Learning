using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ObjectModel
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
