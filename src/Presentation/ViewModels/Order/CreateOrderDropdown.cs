using Domain.Entities;

namespace ViewModels
{
    // todo => remove Entity and add some dto's (or view models idk)
    public class CreateOrderDropdown
    {
        public ICollection<PaymentType> PaymentTypes;
        public ICollection<Meal> Meals;
        public ICollection<Customer> Customers;
    }
}