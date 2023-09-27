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
        private readonly ISessionService _sessionService;

        public OrderController(IOrderService orderService,
                               ISessionService sessionService)
        {
            _orderService = orderService;
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Orders()
        {
            User authenticatedUser = _sessionService.RetrieveUserSession();

            // Getting today orders
            BaseResponse<Order> orders = await _orderService.GetOrdersByDateAsync(authenticatedUser.CompanyId, DateTime.Today.Date);

            List<GetOrderViewModel> getOrderViewModelsCollection = new();
            foreach (Order order in orders.DataCollection)
            {
                GetOrderViewModel getOrderViewModel = new()
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

                getOrderViewModelsCollection.Add(getOrderViewModel);
            }

            return View(getOrderViewModelsCollection);
        }

        [HttpGet]
        [Route("filtering/")]
        public async Task<IActionResult> FilteringOrders(DateTime? createdAt)
        {
            User authenticatedUser = _sessionService.RetrieveUserSession();

            // Filter by date has been applied on OrderService
            BaseResponse<Order> orders = await _orderService.GetOrdersByDateAsync(authenticatedUser.CompanyId, createdAt);

            List<GetOrderViewModel> getOrderViewModelCollection = new();
            foreach (Order order in orders.DataCollection)
            {
                GetOrderViewModel getOrderViewModel = new()
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

                getOrderViewModelCollection.Add(getOrderViewModel);
            }

            return PartialView("_OrdersTable", getOrderViewModelCollection);
        }

        [HttpGet]
        [Route("novo/")]
        public async Task<IActionResult> Create()
        {
            User authenticatedUser = _sessionService.RetrieveUserSession();

            var orderItems = await _orderService.GetCreateOrdersItemsAsync(authenticatedUser.CompanyId);

            CreateOrderDropdown createOrderDropdown = new()
            {
                PaymentTypes = orderItems.Data.PaymentTypes,
                Customers = orderItems.Data.Customers,
                Meals = orderItems.Data.Meals
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
                    return RedirectToAction(nameof(Orders));
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
                    return RedirectToAction(nameof(Orders));
                }
            }

            return RedirectToAction(nameof(Orders));
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
                    GetOrderViewModel getOrderViewModel = new()
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

                    return PartialView("_Details", getOrderViewModel);
                }
            }

            return RedirectToAction(nameof(Orders));
        }

        [HttpDelete]
        [Route("deletar/{orderId}")]
        public async Task<IActionResult> Delete(int orderId)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<Order> result = await _orderService.DeleteOrderAsync(orderId, authenticatedUser);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false, message = "Falha ao excluir o pedido." });
        }

        [HttpGet]
        [Route("editar/{orderId}")]
        public async Task<IActionResult> Update(int orderId)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<Order> result = await _orderService.GetOrderDetailsAsync(orderId, authenticatedUser);

                if (result.IsSuccess)
                {
                    BaseResponse<GetCreateOrderItemsDto> orderItems = await _orderService.GetCreateOrdersItemsAsync(authenticatedUser.CompanyId);

                    CreateOrderDropdown updateOrderDropdown = new()
                    {
                        PaymentTypes = orderItems.Data.PaymentTypes,
                        Customers = orderItems.Data.Customers,
                        Meals = orderItems.Data.Meals
                    };

                    UpdateOrderViewModel updateOrderViewModel = new()
                    {
                        Id = result.Data.Id,
                        Description = result.Data.Description,
                        Price = result.Data.Price,

                        PaymentTypeId = result.Data.PaymentType.Id,
                        MealId = result.Data.Meal.Id,
                        CustomerId = result.Data.Customer.Id,

                        IsPaid = result.Data.IsPaid,
                        PaidAt = result.Data.PaidAt,
                        CreatedAt = result.Data.CreatedAt,
                        UpdatedAt = result.Data.UpdatedAt,

                        CreateOrderDropdown = updateOrderDropdown,
                    };

                    return View(updateOrderViewModel);
                }
            }

            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar/{orderId}")]
        public async Task<IActionResult> Update(int orderId, UpdateOrderViewModel updateOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();

                UpdateOrderDto updateOrderDto = new()
                {
                    Description = updateOrderViewModel.Description,
                    CustomerId = updateOrderViewModel.CustomerId,
                    MealId = updateOrderViewModel.MealId,
                    PaymentTypeId = updateOrderViewModel.PaymentTypeId,
                    Price = updateOrderViewModel.Price,

                    IsPaid = updateOrderViewModel.IsPaid,
                    PaidAt = updateOrderViewModel.PaidAt,

                    OrderId = orderId,
                    CompanyId = authenticatedUser.CompanyId,
                    UserId = authenticatedUser.Id
                };

                BaseResponse<Order> result = await _orderService.UpdateOrderAsync(updateOrderDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = result.IsSuccess;

                    return RedirectToAction(nameof(Orders));
                }
            }

            return View();
        }
    }
}