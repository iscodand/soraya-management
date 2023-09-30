using System.ComponentModel.DataAnnotations.Schema;
using SorayaManagement.Domain.Entities.Common;

namespace SorayaManagement.Domain.Entities
{
    [Table("MEALS")]
    public class Meal : BaseEntity
    {
        public string Description { get; set; }
        public string Accompaniments { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public ICollection<Order> Orders { get; } = new List<Order>();
    }
}