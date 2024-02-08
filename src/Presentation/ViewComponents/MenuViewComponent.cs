using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.Menu;
using Application.DTOs.Authentication;
using Application.Contracts.Services;

namespace Presentation.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ISessionService _sessionService;

        public MenuViewComponent(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            GetAuthenticatedUserDto authenticatedUser = _sessionService.RetrieveUserSession();

            GetUserDataViewModel viewModel = new()
            {
                Name = authenticatedUser.Name,
                CompanyName = authenticatedUser.CompanyName,
            };

            return View(viewModel);
        }
    }
}