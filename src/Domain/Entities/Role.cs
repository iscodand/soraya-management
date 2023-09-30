using Microsoft.AspNetCore.Identity;

namespace SorayaManagement.Domain.Entities
{
    public class Role : IdentityRole<string>
    {
        public string Description { get; set; }
    }
}
