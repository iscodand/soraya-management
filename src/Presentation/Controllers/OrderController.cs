using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos.Order;
using Application.Wrappers;
using Presentation.ViewModels.Order;
using Presentation.Controllers.Common;
using Application.DTOs.Authentication;
using Application.Contracts.Services;
using Application.Parameters;

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
        public async Task<IActionResult> Orders(int pageNumber = 1)
        {
            RequestParameter parameters = new()
            {
                PageNumber = pageNumber,
                PageSize = 10,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now
            };

            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

<<<<<<< HEAD
            // Getting today orders
            Response<IEnumerable<GetOrderDto>> orders = await _orderService.GetOrdersByDateAsync(authenticatedUser.CompanyId, DateTime.Today.Date);

            List<GetOrderViewModel> getOrderViewModelsCollection = new();
            foreach (GetOrderDto order in orders.Data)
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
=======
            var result = await _orderService.GetOrdersByDateRangePagedAsync(
                authenticatedUser.CompanyId,
                parameters
            );

            return View(result);
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
        }

        [HttpGet]
        [Route("buscar/")]
        public async Task<IActionResult> FilteringOrders(DateTime? createdAt)
        {
            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

            // Filter by date has been applied on OrderService
            Response<IEnumerable<GetOrderDto>> orders = await _orderService.GetOrdersByDateAsync(authenticatedUser.CompanyId, createdAt);

            List<GetOrderViewModel> getOrderViewModelCollection = new();
            foreach (GetOrderDto order in orders.Data)
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

            Response<GetCreateOrderItemsDto> orderItems = await _orderService.GetCreateOrdersItemsAsync(authenticatedUser.CompanyId);

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

                Response<CreateOrderDto> result = await _orderService.CreateOrderAsync(createOrderDto);
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    ViewData["Succeeded"] = true;
                    return RedirectToAction(nameof(Orders));
                }
            }

            return View();
        }

        [HttpPost]
        [Route("{orderId}/marcar-como-pago")]
        public async Task<IActionResult> MarkOrderAsPaid(int orderId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<UpdateOrderDto> result = await _orderService.MakeOrderPaymentAsync(orderId, authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao atualizar o pedido." });
        }

        [HttpGet]
<<<<<<< HEAD
        [Route("detalhes/{orderId}")]
        public async Task<IActionResult> Detail(int orderId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<DetailOrderDto> result = await _orderService.GetOrderDetailsAsync(orderId, authenticatedUser.CompanyId);

                if (result.Succeeded)
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
                Response<GetOrderDto> result = await _orderService.DeleteOrderAsync(orderId, authenticatedUser.CompanyId);
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    ViewData["Succeeded"] = result.Succeeded;
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao excluir o pedido." });
        }

        [HttpGet]
        [Route("editar/{orderId}")]
=======
        [Route("{orderId}")]
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
        public async Task<IActionResult> Update(int orderId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<DetailOrderDto> result = await _orderService.GetOrderDetailsAsync(orderId, authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
<<<<<<< HEAD
                    Response<GetCreateOrderItemsDto> orderItems = await _orderService.GetCreateOrdersItemsAsync(authenticatedUser.CompanyId);
=======
                    var dropdownResult = await _orderService.GetCreateOrdersItemsAsync(authenticatedUser.CompanyId);
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650

                    CreateOrderDropdown dropdownViewModel = new()
                    {
                        Customers = dropdownResult.Data.Customers,
                        Meals = dropdownResult.Data.Meals,
                        PaymentTypes = dropdownResult.Data.PaymentTypes
                    };

                    UpdateOrderViewModel viewModel = UpdateOrderViewModel.Map(result.Data, dropdownViewModel);

                    return View(viewModel);
                }
            }

            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{orderId}")]
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

                Response<UpdateOrderDto> result = await _orderService.UpdateOrderAsync(updateOrderDto);
<<<<<<< HEAD
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    ViewData["Succeeded"] = result.Succeeded;
                    return RedirectToAction(nameof(Orders));
=======

                ViewData["Message"] = result.Message;
                ViewData["Succeeded"] = result.Succeeded;

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Update), new { OrderId = orderId });
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
                }
            }

            return RedirectToAction(nameof(Update), new { OrderId = orderId });
        }

        [HttpDelete]
        [Route("deletar/{orderId}")]
        public async Task<IActionResult> Delete(int orderId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<GetOrderDto> result = await _orderService.DeleteOrderAsync(orderId, authenticatedUser.CompanyId);
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    ViewData["Succeeded"] = result.Succeeded;
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao excluir o pedido." });
        }
    }
}