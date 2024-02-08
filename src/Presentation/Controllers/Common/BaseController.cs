using Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Common
{
    public class BaseController : Controller
    {
        private ISessionService _sessionService;
        protected ISessionService SessionService => _sessionService ??= HttpContext.RequestServices.GetService<ISessionService>();
    }
}