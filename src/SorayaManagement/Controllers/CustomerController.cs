using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Application.Contracts;
using SorayaManagement.Application.Dtos.Customer;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Infrastructure.Identity.Responses;
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
            ICollection<Customer> customers = await _customerService.GetCustomersByCompanyAsync(authenticatedUser.CompanyId);

            // todo => improve this logic (maybe with automapper inside service layer)
            List<GetCustomersViewModel> viewModelCollection = new();
            foreach (Customer customer in customers)
            {
                GetCustomersViewModel viewModel = new()
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
        public async Task<IActionResult> Create(CreateCustomerDto createCustomerDto)
        {
            if (ModelState.IsValid)
            {
                User authenticatedUser = _sessionService.RetrieveUserSession();
                BaseResponse response = await _customerService.CreateCustomerAsync(createCustomerDto, authenticatedUser);
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