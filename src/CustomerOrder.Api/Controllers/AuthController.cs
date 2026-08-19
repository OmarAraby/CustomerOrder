using CustomerOrder.Application.Common;
using CustomerOrder.Application.Dtos.Auth;
using CustomerOrder.Application.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace CustomerOrder.Api.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(LoginDto request)
        {
            var token = await _authService.LoginAsync(request);

            return Ok(ApiResponse<TokenDto>.SuccessResponse(token, "Login successful"));
        }
    }
}