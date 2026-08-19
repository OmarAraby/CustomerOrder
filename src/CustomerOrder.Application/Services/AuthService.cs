using CustomerOrder.Application.Dtos.Auth;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Core.Exceptions;
using System.Threading.Tasks;

namespace CustomerOrder.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IIdentityService _identityService;
        private IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IIdentityService identityService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _identityService = identityService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<TokenDto> LoginAsync(LoginDto dto)
        {
            //throw new System.NotImplementedException();

            var user = await _identityService.ValidateCredentialsAsync(dto.UserName, dto.Password);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid username or password.");
            }

            return _jwtTokenGenerator.Generate(user);

        }
    }
}
