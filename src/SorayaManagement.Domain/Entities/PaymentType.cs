using System.ComponentModel.DataAnnotations;
using SorayaManagement.Domain.Entities.Common;

namespace SorayaManagement.Domain.Entities
{
    public class PaymentType
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}