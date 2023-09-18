using SorayaManagement.Domain.Entities.Common;

namespace SorayaManagement.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}