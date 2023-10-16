using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Infrastructure.Identity.Contracts;
using Presentation.ViewModels.Menu;
using Application.Dtos.User;

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