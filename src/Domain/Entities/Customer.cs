using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities
{
    [Table("CUSTOMERS")]
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public ICollection<Order> Orders { get; } = new List<Order>();
    }
}