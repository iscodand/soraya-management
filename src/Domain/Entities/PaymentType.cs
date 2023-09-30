using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("PAYMENT_TYPES")]
    public class PaymentType
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}