using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TH_Lap3.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Role { get; set; }
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }
    }
}
