using Microsoft.AspNetCore.Identity;
 

namespace Fiorella_second.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActivated { get; set; }
    }
}
