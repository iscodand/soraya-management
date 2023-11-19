using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts;
using Application.Dtos.Order;
using Application.Responses;
using Presentation.ViewModels.Order;
using Application.Dtos.User;
using Presentation.Controllers.Common;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("pedidos/")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Orders()
        {
            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

            // Getting today orders
            BaseResponse<GetOrderDto> orders = await _orderService.GetOrdersByDateAsync(authenticatedUser.CompanyId, DateTime.Today.Date);

            List<GetOrderViewModel> getOrderViewModelsCollection = new();
            foreach (GetOrderDto order in orders.DataCollection)
            {
                GetOrderViewModel getOrderViewModel = new()
                {
                    Id = order.Id,
                    Description = order.Description,
                    Price = order.Price,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    PaymentType = order.PaymentType,
                    Meal = order.Meal,
                    Customer = order.Customer,
                    CreatedBy = order.CreatedBy,
                    CreatedAt = order.CreatedAt
                };

                getOrderViewModelsCollection.Add(getOrderViewModel);
            }

            return View(getOrderViewModelsCollection);
        }

        [HttpGet]
        [Route("buscar/")]
        public async Task<IActionResult> FilteringOrders(DateTime? createdAt)
        {
            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

            // Filter by date has been applied on OrderService
            BaseResponse<GetOrderDto> orders = await _orderService.GetOrdersByDateAsync(authenticatedUser.CompanyId, createdAt);

            List<GetOrderViewModel> getOrderViewModelCollection = new();
            foreach (GetOrderDto order in orders.DataCollection)
            {
                GetOrderViewModel getOrderViewModel = new()
                {
                    Id = order.Id,
                    Description = order.Description,
                    Price = order.Price,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    PaymentType = order.PaymentType,
                    Meal = order.Meal,
                    Customer = order.Customer,
                    CreatedBy = order.CreatedBy,
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
            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

            BaseResponse<GetCreateOrderItemsDto> orderItems = await _orderService.GetCreateOrdersItemsAsync(authenticatedUser.CompanyId);

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
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

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

                BaseResponse<CreateOrderDto> result = await _orderService.CreateOrderAsync(createOrderDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return RedirectToAction(nameof(Orders));
                }
            }

            return View();
        }

        [HttpPatch]
        [Route("marcar-como-pago/{orderId}")]
        public async Task<IActionResult> MarkOrderAsPaid(int orderId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                BaseResponse<UpdateOrderDto> result = await _orderService.MakeOrderPaymentAsync(orderId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao atualizar o pedido." });
        }

        [HttpGet]
        [Route("detalhes/{orderId}")]
        public async Task<IActionResult> Detail(int orderId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                BaseResponse<DetailOrderDto> result = await _orderService.GetOrderDetailsAsync(orderId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    GetOrderViewModel getOrderViewModel = new()
                    {
                        Id = result.Data.Id,
                        Description = result.Data.Description,
                        Price = result.Data.Price,
                        IsPaid = result.Data.IsPaid,
                        PaidAt = result.Data.PaidAt,
                        PaymentType = result.Data.PaymentType,
                        Meal = result.Data.Meal,
                        Customer = result.Data.Customer,
                        CreatedBy = result.Data.CreatedBy,
                        CreatedAt = result.Data.CreatedAt
                    };

                    return PartialView("_Detail", getOrderViewModel);
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
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                BaseResponse<GetOrderDto> result = await _orderService.DeleteOrderAsync(orderId, authenticatedUser.CompanyId);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = result.IsSuccess;
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao excluir o pedido." });
        }

        [HttpGet]
        [Route("editar/{orderId}")]
        public async Task<IActionResult> Update(int orderId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                BaseResponse<DetailOrderDto> result = await _orderService.GetOrderDetailsAsync(orderId, authenticatedUser.CompanyId);

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

                        PaymentTypeId = result.Data.PaymentTypeId,
                        MealId = result.Data.MealId,
                        CustomerId = result.Data.CustomerId,

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
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

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

                BaseResponse<UpdateOrderDto> result = await _orderService.UpdateOrderAsync(updateOrderDto);
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