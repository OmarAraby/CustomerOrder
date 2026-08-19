using CustomerOrder.Application.Dtos.Auth;

namespace CustomerOrder.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        TokenDto Generate(AuthenticatedUser user);
    }
}
