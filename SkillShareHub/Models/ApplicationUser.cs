using Microsoft.AspNetCore.Identity;

namespace SkillShareHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom fields here (optional now, future-proof for later)
        public string FullName { get; set; }
        
    }
}
