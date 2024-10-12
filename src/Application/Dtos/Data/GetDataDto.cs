using Application.Dtos.Customer;
using Application.Dtos.Meal;
using Application.Dtos.Order;

namespace Application.Dtos.Data
{
    public class GetDataDto
    {
        public IEnumerable<GetOrderDto> Orders { get; set; }
        public ICollection<IGrouping<DateTime, GetOrderDto>> OrdersByDayOfWeek { get; set; }
        public IEnumerable<GetMealDto> Meals { get; set; }
        public IEnumerable<GetCustomerDto> Customers { get; set; }
    }
}