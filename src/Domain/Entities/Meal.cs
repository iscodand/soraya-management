using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities
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

        public static Meal Create(string description,
                                  string accompaniments,
                                  int companyId,
                                  string createdById)
        {
            Meal meal = new()
            {
                Description = description,
                Accompaniments = accompaniments,
                CompanyId = companyId,
                UserId = createdById
            };

            return meal;
        }
    }
}