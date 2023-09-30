using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Infrastructure.Identity.Contracts;
using UI.Menu.ViewModels;

namespace ViewComponents
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
            User authenticatedUser = _sessionService.RetrieveUserSession();

            GetUserDataViewModel viewModel = new()
            {
                Name = authenticatedUser.Name,
                CompanyName = authenticatedUser.UserCompany.Name,
            };

            return View(viewModel);
        }
    }
}