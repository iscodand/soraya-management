using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts;
using Application.Dtos.Customer;
using Application.Responses;
using Infrastructure.Identity.Contracts;
using Presentation.ViewModels.Customer;
using Presentation.ViewModels.Order;
using Application.Dtos.Order;
using Application.Dtos.User;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("clientes")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ISessionService _sessionService;

        public CustomerController(ICustomerService customerService,
                                  ISessionService sessionService)
        {
            _customerService = customerService;
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Customers()
        {
            GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
            BaseResponse<GetCustomerDto> customers = await _customerService.GetCustomersByCompanyAsync(authenticatedUser.CompanyId);

            List<GetCustomerViewModel> getCustomerDtoCollection = new();
            foreach (GetCustomerDto customer in customers.DataCollection)
            {
                GetCustomerViewModel viewModel = new()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    IsActive = customer.IsActive,
                    CreatedBy = customer.CreatedBy
                };

                getCustomerDtoCollection.Add(viewModel);
            }

            return View(getCustomerDtoCollection.OrderBy(x => x.IsActive == false).ToList());
        }

        [HttpGet]
        [Route("novo/")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("novo/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel createCustomerViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();

                CreateCustomerDto createCustomerDto = new()
                {
                    Name = createCustomerViewModel.Name,
                    Phone = createCustomerViewModel.Phone,
                    UserId = authenticatedUser.Id,
                    CompanyId = authenticatedUser.CompanyId
                };

                BaseResponse<CreateCustomerDto> response = await _customerService.CreateCustomerAsync(createCustomerDto);
                ViewData["Message"] = response.Message;

                if (response.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return RedirectToAction(nameof(Customers));
                }
            }

            return View();
        }

        [HttpGet]
        [Route("detalhes/{customerId}")]
        public async Task<IActionResult> Details(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<DetailCustomerDto> result = await _customerService.DetailCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    List<GetOrderViewModel> getOrderViewModelCollection = new();
                    foreach (GetOrderDto order in result.Data.Orders)
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
                            CreatedAt = order.CreatedAt
                        };

                        getOrderViewModelCollection.Add(getOrderViewModel);
                    }

                    DetailCustomerViewModel detailCustomerViewModel = new()
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name,
                        Phone = result.Data.Phone,
                        IsActive = result.Data.IsActive,
                        CreatedBy = result.Data.CreatedBy,
                        Orders = getOrderViewModelCollection
                    };

                    return View(detailCustomerViewModel);
                }
            }

            return RedirectToAction(nameof(Customers));
        }

        [HttpGet]
        [Route("editar/{customerId}")]
        public async Task<IActionResult> Update(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<GetCustomerDto> result = await _customerService.GetCustomerByIdAsync(customerId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    UpdateCustomerViewModel updateCustomerViewModel = new()
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name,
                        Phone = result.Data.Phone
                    };

                    return View(updateCustomerViewModel);
                }
            }

            return RedirectToAction(nameof(Customers));
        }

        [HttpPost]
        [Route("editar/{customerId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int customerId, UpdateCustomerViewModel updateCustomerViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();

                UpdateCustomerDto updateCustomerDto = new()
                {
                    Id = customerId,
                    Name = updateCustomerViewModel.Name,
                    Phone = updateCustomerViewModel.Phone,
                    UserCompanyId = authenticatedUser.CompanyId
                };

                BaseResponse<UpdateCustomerDto> result = await _customerService.UpdateCustomerAsync(updateCustomerDto);
                ViewData["Message"] = result.Message;

                if (result.IsSuccess)
                {
                    ViewData["IsSuccess"] = result.IsSuccess;
                    return RedirectToAction(nameof(Customers));
                }
            }

            return View();
        }

        [HttpPatch]
        [Authorize(Roles = "Manager, Admin")]
        [Route("ativar/{customerId}")]
        public async Task<IActionResult> Activate(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<UpdateCustomerDto> result = await _customerService.ActivateCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao ativar cliente." });
        }

        [HttpPatch]
        [Authorize(Roles = "Manager, Admin")]
        [Route("desativar/{customerId}")]
        public async Task<IActionResult> Inactivate(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<UpdateCustomerDto> result = await _customerService.InactivateCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao desativar cliente." });
        }

        [HttpDelete]
        [Authorize(Roles = "Manager, Admin")]
        [Route("deletar/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<GetCustomerDto> result = await _customerService.DeleteCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao deletar cliente." });
        }
    }
}