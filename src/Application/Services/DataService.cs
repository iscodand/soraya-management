using Application.Contracts;
using Application.Dtos.Customer;
using Application.Dtos.Data;
using Application.Dtos.Meal;
using Application.Dtos.Order;
using Application.Responses;

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

        public async Task<BaseResponse<GetDataDto>> GetDataAsync(int companyId, DateTime? initialDate, DateTime? finalDate)
        {
            BaseResponse<GetOrderDto> orders = await _orderService.GetOrdersByDateRangeAsync(companyId, initialDate, finalDate);
            BaseResponse<GetMealDto> meals = await _mealService.GetMealsByCompanyAsync(companyId);
            BaseResponse<GetCustomerDto> customers = await _customerService.GetCustomersByCompanyAsync(companyId);

            var ordersPerDay = orders.DataCollection.GroupBy(x => x.CreatedAt.Day).Select(x => x.Key);

            GetDataDto getDataDto = new()
            {
                Orders = orders.DataCollection,
                Meals = meals.DataCollection,
                Customers = customers.DataCollection
            };

            return new BaseResponse<GetDataDto>()
            {
                Message = "Dados recuperados com sucesso",
                IsSuccess = true,
                Data = getDataDto
            };
        }
    }
}