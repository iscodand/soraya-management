using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SorayaManagement.Domain.Entities
{
    [Table("USERS")]
    public class User : IdentityUser<string>
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public Company UserCompany { get; set; }
        public int CompanyId { get; set; }

        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}