using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Order;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.ViewModels;

namespace SorayaManagement.Controllers
{
    [Authorize]
    [Route("pedidos/")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IMealService _mealService;
        private readonly ISessionService _sessionService;

        public OrderController(IOrderService orderService,
                               ICustomerService customerService,
                               IMealService mealService,
                               ISessionService sessionService)
        {
            _orderService = orderService;
            _sessionService = sessionService;
            _customerService = customerService;
            _mealService = mealService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Orders(string isPaid)
        {
            User authenticatedUser = _sessionService.RetrieveUserSession();

            ICollection<Order> orders = await _orderService.GetOrdersByCompanyAsync(authenticatedUser.CompanyId);
            List<GetOrdersViewModel> viewModelCollection = new();

            foreach (Order order in orders)
            {
                GetOrdersViewModel viewModel = new()
                {
                    Id = order.Id,
                    Description = order.Description,
                    Price = order.Price,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    PaymentType = order.PaymentType.Description,
                    Meal = order.Meal.Description,
                    Customer = order.Customer.Name,
                    CreatedBy = order.User.Name,
                    CreatedAt = order.CreatedAt
                };

                viewModelCollection.Add(viewModel);
            }

            // filtering stuff 
            // => maybe this should not be here
            // => verify the best implementation for this
            switch (isPaid)
            {
                case "true":
                    viewModelCollection = viewModelCollection.Where(x => x.IsPaid == true).ToList();
                    break;
                case "false":
                    viewModelCollection = viewModelCollection.Where(x => x.IsPaid == false).ToList();
                    break;
                case "all":
                    break;
            }

            return View(viewModelCollection);
        }

        [HttpGet]
        [Route("novo/")]
        public async Task<IActionResult> Create()
        {
            User authenticatedUser = _sessionService.RetrieveUserSession();

            ICollection<PaymentType> paymentTypes = await _orderService.GetPaymentTypesAsync();
            ICollection<Customer> customers = await _customerService.GetCustomersByCompanyAsync(authenticatedUser.CompanyId);
            ICollection<Meal> meals = await _mealService.GetMealsByCompanyAsync(authenticatedUser.CompanyId);

            CreateOrderDropdown createOrderDropdown = new()
            {
                PaymentTypes = paymentTypes,
                Customers = customers,
                Meals = meals
            };

            CreateOrderDto createOrderDto = new()
            {
                CreateOrderDropdown = createOrderDropdown
            };

            return View(createOrderDto);
        }

        [HttpPost]
        [Route("novo/")]
        public async Task<IActionResult> Create(CreateOrderDto createOrderDto)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<Order> result = await _orderService.CreateOrderAsync(createOrderDto, authenticatedUser);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return RedirectToAction("Orders");
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkOrderAsPaid(int orderId)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<Order> result = await _orderService.MakeOrderPaymentAsync(orderId, authenticatedUser);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return RedirectToAction("Orders");
                }
            }

            return RedirectToAction("Orders");
        }

        [HttpGet]
        [Route("detalhes/{orderId}")]
        public async Task<IActionResult> Details(int orderId)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<Order> result = await _orderService.GetOrderDetailsAsync(orderId, authenticatedUser);

                if (result.IsSuccess)
                {
                    return View(result.Data);
                }
            }

            return RedirectToAction(nameof(Orders));
        }
    }
}