using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts;
using Application.Dtos.Customer;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Identity.Contracts;
using Presentation.ViewModels.Customer;

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
            User authenticatedUser = _sessionService.RetrieveUserSession();
            BaseResponse<Customer> customers = await _customerService.GetCustomersByCompanyAsync(authenticatedUser.CompanyId);

            List<GetCustomerViewModel> viewModelCollection = new();
            foreach (Customer customer in customers.DataCollection)
            {
                GetCustomerViewModel viewModel = new()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    IsActive = customer.IsActive,
                    CreatedBy = customer.User.Name
                };

                viewModelCollection.Add(viewModel);
            }

            return View(viewModelCollection);
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
                User authenticatedUser = _sessionService.RetrieveUserSession();

                CreateCustomerDto createCustomerDto = new()
                {
                    Name = createCustomerViewModel.Name,
                    Phone = createCustomerViewModel.Phone,
                    UserId = authenticatedUser.Id,
                    CompanyId = authenticatedUser.CompanyId
                };

                BaseResponse<Customer> response = await _customerService.CreateCustomerAsync(createCustomerDto);
                ViewData["Message"] = response.Message;

                if (response.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return View(nameof(Customers));
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
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<Customer> result = await _customerService.DetailCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    DetailCustomerViewModel detailCustomerViewModel = new()
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name,
                        Phone = result.Data.Phone,
                        IsActive = result.Data.IsActive,
                        CreatedBy = result.Data.User.Name,
                        Orders = result.Data.Orders
                    };

                    return View(detailCustomerViewModel);
                }
            }

            return RedirectToAction(nameof(Customers));
        }

        [HttpPatch]
        [Route("desativar/{customerId}")]
        public async Task<IActionResult> Inactivate(int customerId)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<Customer> result = await _customerService.InactivateCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao desativar cliente." });
        }

        [HttpPatch]
        [Route("ativar/{customerId}")]
        public async Task<IActionResult> Activate(int customerId)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse<Customer> result = await _customerService.ActivateCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao ativar cliente." });
        }
    }
}