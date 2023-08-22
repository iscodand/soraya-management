using Microsoft.AspNetCore.Identity;

namespace SorayaManagement.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Company UserCompany { get; set; }
        public int CompanyId { get; set; }

        public User()
        {
            NormalizedName = Name.Trim().ToUpper();
        }
    }
}