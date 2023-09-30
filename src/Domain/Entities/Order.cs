using System.ComponentModel.DataAnnotations.Schema;
using SorayaManagement.Domain.Entities.Common;

namespace SorayaManagement.Domain.Entities
{
    [Table("ORDERS")]
    public class Order : BaseEntity
    {
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }

        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentType { get; set; }
        public int PaymentTypeId { get; set; }

        [ForeignKey("MealId")]
        public Meal Meal { get; set; }
        public int MealId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public int CompanyId { get; set; }
    }
}