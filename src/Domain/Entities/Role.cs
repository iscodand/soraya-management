using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Role : IdentityRole<string>
    {
        public string Description { get; set; }
    }
}
