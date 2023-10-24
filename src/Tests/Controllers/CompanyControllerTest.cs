using Application.Contracts;
using Infrastructure.Identity.Contracts;
using Presentation.Controllers;

namespace Tests.Controllers
{
    public class CompanyControllerTest
    {
        private readonly CompanyController _companyController;
        private readonly ISessionService _sessionService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public CompanyControllerTest()
        {
            _sessionService = A.Fake<ISessionService>();
            _authenticationService = A.Fake<IAuthenticationService>();
            _userService = A.Fake<IUserService>();
            _companyController = new CompanyController(_sessionService, _authenticationService, _userService);
        }
    }
}