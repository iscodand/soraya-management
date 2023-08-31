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
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public CustomerController(ICustomerService customerService,
                                                  IAuthenticatedUserService authenticatedUserService)
        {
            _customerService = customerService;
            _authenticatedUserService = authenticatedUserService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Customers()
        {
            User authenticatedUser = await _authenticatedUserService.GetAuthenticatedUserAsync();
            ICollection<Customer> customers = await _customerService.GetCustomersByCompanyAsync(authenticatedUser.CompanyId);

            // todo => improve this logic (maybe with automapper inside service layer)
            List<GetCustomersViewModel> viewModelCollection = new();
            foreach (Customer customer in customers)
            {
                GetCustomersViewModel viewModel = new()
                {
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
        public async Task<IActionResult> Create(CreateCustomerDto createCustomerDto)
        {
            User authenticatedUser = await _authenticatedUserService.GetAuthenticatedUserAsync();
            createCustomerDto.CreatedBy = authenticatedUser.Id;

            if (ModelState.IsValid)
            {
                BaseResponse response = await _customerService.CreateCustomerAsync(createCustomerDto);
                ViewData["Message"] = response.Message;

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Customer));
                }
            }

            return View();
        }
    }
}