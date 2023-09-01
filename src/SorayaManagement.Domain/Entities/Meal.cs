using SorayaManagement.Domain.Entities.Common;

namespace SorayaManagement.Domain.Entities
{
    public class Meal : BaseEntity
    {
        public string Description { get; set; }
        public string Accompaniments { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}