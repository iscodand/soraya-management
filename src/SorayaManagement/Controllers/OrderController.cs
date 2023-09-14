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
        public async Task<IActionResult> Orders()
        {
            User authenticatedUser = _sessionService.RetrieveUserSession();

            // todo => is loading ALL orders by company - try to load just today orders first
            ICollection<Order> orders = await _orderService.GetOrdersByCompanyAsync(authenticatedUser.CompanyId);
            List<GetOrderViewModel> viewModelCollection = new();
            foreach (Order order in orders)
            {
                GetOrderViewModel viewModel = new()
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

            return View(viewModelCollection);
        }

        [HttpGet]
        [Route("filtering/")]
        public async Task<IActionResult> FilteringOrders(string isPaid, DateTime? createdAt)
        {
            // // filtering stuff
            // // => maybe this should not be here
            // // => verify the best implementation for this
            User authenticatedUser = _sessionService.RetrieveUserSession();

            ICollection<Order> orders = await _orderService.GetOrdersByCompanyAsync(authenticatedUser.CompanyId);

            List<GetOrderViewModel> viewModelCollection = new();
            foreach (Order order in orders)
            {
                GetOrderViewModel viewModel = new()
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

            if (createdAt != null)
            {
                viewModelCollection = viewModelCollection.Where(x => x.CreatedAt.Date == createdAt.Value.Date)
                                                         .ToList();
            }

            return PartialView("_OrdersTable", viewModelCollection);
        }

        [HttpGet]
        [Route("novo/")]
        public async Task<IActionResult> Create()
        {
            User authenticatedUser = _sessionService.RetrieveUserSession();

            BaseResponse<PaymentType> paymentTypes = await _orderService.GetPaymentTypesAsync();
            BaseResponse<Customer> customers = await _customerService.GetCustomersByCompanyAsync(authenticatedUser.CompanyId);
            BaseResponse<Meal> meals = await _mealService.GetMealsByCompanyAsync(authenticatedUser.CompanyId);

            CreateOrderDropdown createOrderDropdown = new()
            {
                PaymentTypes = paymentTypes.DataCollection,
                Customers = customers.DataCollection,
                Meals = meals.DataCollection
            };

            CreateOrderViewModel createOrderViewModel = new()
            {
                CreateOrderDropdown = createOrderDropdown
            };

            // returning ViewModel instead dropdown directly to avoid some View problems
            // maybe it's incorrect, but whatever, this works
            return View(createOrderViewModel);
        }

        [HttpPost]
        [Route("novo/")]
        public async Task<IActionResult> Create(CreateOrderViewModel createOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();

                CreateOrderDto createOrderDto = new()
                {
                    Description = createOrderViewModel.Description,
                    CustomerId = createOrderViewModel.CustomerId,
                    MealId = createOrderViewModel.MealId,
                    PaymentTypeId = createOrderViewModel.PaymentTypeId,
                    Price = createOrderViewModel.Price,
                    CompanyId = authenticatedUser.CompanyId,
                    UserId = authenticatedUser.Id
                };

                BaseResponse<Order> result = await _orderService.CreateOrderAsync(createOrderDto);
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
                    GetOrderViewModel order = new()
                    {
                        Id = result.Data.Id,
                        Description = result.Data.Description,
                        Price = result.Data.Price,
                        IsPaid = result.Data.IsPaid,
                        PaidAt = result.Data.PaidAt,
                        PaymentType = result.Data.PaymentType.Description,
                        Meal = result.Data.Meal.Description,
                        Customer = result.Data.Customer.Name,
                        CreatedBy = result.Data.User.Name,
                        CreatedAt = result.Data.CreatedAt
                    };

                    return View(order);
                }
            }

            return RedirectToAction(nameof(Orders));
        }
    }
}