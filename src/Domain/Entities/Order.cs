using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities
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

        public static Order Create(string description,
                                  decimal price,
                                  int paymentTypeId,
                                  int mealId,
                                  int customerId,
                                  int companyId,
                                  string createdById)
        {
            Order order = new()
            {
                Description = description,
                Price = price,
                IsPaid = false,
                PaidAt = null,
                PaymentTypeId = paymentTypeId,
                MealId = mealId,
                CustomerId = customerId,
                CompanyId = companyId,
                UserId = createdById
            };

            return order;
        }

        public Order MarkAsPaid(int userCompanyId)
        {
            if (CompanyId != userCompanyId)
            {
                throw new Exception("Você não pode atualizar pedidos de outras empresas.");
            }

            IsPaid = true;
            PaidAt = DateTime.Now;

            return this;
        }
    }
}