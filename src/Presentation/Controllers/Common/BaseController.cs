using Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Common
{
    public class BaseController : Controller
    {
        private ISessionService _sessionService;
        protected ISessionService SessionService => _sessionService ??= HttpContext.RequestServices.GetService<ISessionService>();

        protected IAuthenticatedUserService _authenticatedUserService;
        protected IAuthenticatedUserService AuthenticatedUser => _authenticatedUserService ??= HttpContext.RequestServices.GetService<IAuthenticatedUserService>();
    }
}