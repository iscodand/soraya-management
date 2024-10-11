using Application.Contracts.Services;
using Application.Dtos.Customer;
using Application.Dtos.Data;
using Application.Dtos.Meal;
using Application.Dtos.Order;
using Application.Wrappers;

namespace Application.Services
{
    public class DataService : IDataService
    {
        private readonly IOrderService _orderService;
        private readonly IMealService _mealService;
        private readonly ICustomerService _customerService;

        public DataService(IOrderService orderService,
                           IMealService mealService,
                           ICustomerService customerService)
        {
            _orderService = orderService;
            _mealService = mealService;
            _customerService = customerService;
        }

        public async Task<Response<GetDataDto>> GetDataAsync(int companyId, DateTime initialDate, DateTime finalDate)
        {
            Response<IEnumerable<GetOrderDto>> orders = await _orderService.GetOrdersByDateRangeAsync(companyId, initialDate, finalDate);

            // todo => select just the last 6 meals/customers with the higher num of orders at range of initial and final date
            Response<IEnumerable<GetMealDto>> meals = await _mealService.GetMealsByDateRangeAsync(companyId, initialDate, finalDate);
            Response<IEnumerable<GetCustomerDto>> customers = await _customerService.GetCustomersByDateRangeAsync(companyId, initialDate, finalDate);

            GetDataDto getDataDto = new()
            {
                Orders = orders.Data,
                Meals = meals.Data,
                Customers = customers.Data
            };

            return new()
            {
                Message = "Dados recuperados com sucesso",
                Succeeded = true,
                Data = getDataDto
            };
        }
    }
}