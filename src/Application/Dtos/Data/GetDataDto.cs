using Application.Dtos.Customer;
using Application.Dtos.Meal;
using Application.Dtos.Order;

namespace Application.Dtos.Data
{
    public class GetDataDto
    {
        public ICollection<GetOrderDto> Orders { get; set; }
        public IEnumerable<IGrouping<DateTime, GetOrderDto>> OrdersByDayOfWeek { get; set; }
        public ICollection<GetMealDto> Meals { get; set; }
        public ICollection<GetCustomerDto> Customers { get; set; }
    }
}