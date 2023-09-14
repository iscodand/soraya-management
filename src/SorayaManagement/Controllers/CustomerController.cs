using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Customer;
using SorayaManagement.Application.Responses;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.ViewModels;

namespace SorayaManagement.Controllers
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
                    UserId = authenticatedUser.Id,
                    CompanyId = authenticatedUser.CompanyId
                };

                BaseResponse<Customer> response = await _customerService.CreateCustomerAsync(createCustomerDto);
                ViewData["Message"] = response.Message;

                if (response.IsSuccess)
                {
                    ViewData["IsSuccess"] = true;
                    return View();
                }
            }

            return View();
        }
    }
}