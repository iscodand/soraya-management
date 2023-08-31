using Microsoft.AspNetCore.Mvc;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;

namespace SorayaManagement.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public MenuViewComponent(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            User authenticatedUser = await _authenticatedUserService.GetAuthenticatedUserAsync();
            return View(authenticatedUser);
        }
    }
}