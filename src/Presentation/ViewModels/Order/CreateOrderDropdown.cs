using Domain.Entities;

namespace Presentation.ViewModels.Order
{
    // todo => remove Entity and add some dto's (or view models idk)
    public class CreateOrderDropdown
    {
        public ICollection<PaymentType> PaymentTypes;
        public ICollection<Domain.Entities.Meal> Meals;
        public ICollection<Domain.Entities.Customer> Customers;
    }
}