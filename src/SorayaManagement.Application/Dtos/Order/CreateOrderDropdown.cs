using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Application.Dtos.Order
{
    public class CreateOrderDropdown
    {
        public ICollection<PaymentType> PaymentTypes;
        public ICollection<Domain.Entities.Meal> Meals;
        public ICollection<Domain.Entities.Customer> Customers;
    }
}