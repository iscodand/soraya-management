using System.ComponentModel.DataAnnotations.Schema;
using SorayaManagement.Domain.Entities.Common;

namespace SorayaManagement.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PaymentType PaymentType { get; set; }
        public int PaymentTypeId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Meal Meal { get; set; }
        public int MealId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }
    }
}