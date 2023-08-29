using System.ComponentModel.DataAnnotations;

namespace SorayaManagement.Domain.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Meal> Meals { get; set; }

        public DateTime CreatedAt { get; set; }

        public Company()
        {
            CreatedAt = DateTime.Now;
        }
    }
}