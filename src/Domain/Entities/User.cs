using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
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

        public static User Create(string name,
                                  string email,
                                  string username,
                                  int companyId)
        {
            User user = new()
            {
                Name = name,
                NormalizedName = name.Trim().ToUpper(),
                Email = email,
                UserName = username,
                CompanyId = companyId
            };

            return user;
        }
    }
}