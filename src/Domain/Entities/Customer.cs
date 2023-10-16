using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities
{
    [Table("CUSTOMERS")]
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InactivatedAt { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public ICollection<Order> Orders { get; } = new List<Order>();

        public static Customer Create(string name, string phone, int companyId, string userId)
        {
            Customer customer = new()
            {
                Name = name,
                Phone = phone,
                IsActive = true,
                InactivatedAt = null,
                CompanyId = companyId,
                UserId = userId
            };

            return customer;
        }

        public Customer Update(string name, string phone)
        {
            Name = name;
            Phone = phone;
            UpdatedAt = DateTime.Now;
            return this;
        }

        public Customer Activate()
        {
            IsActive = true;
            InactivatedAt = null;
            return this;
        }

        public Customer Inactivate()
        {
            IsActive = false;
            InactivatedAt = DateTime.Now;
            return this;
        }
    }
}