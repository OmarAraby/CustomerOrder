using CustomerOrder.Application.Dtos.Auth;
using System.Threading.Tasks;

namespace CustomerOrder.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto dto);
    }
}
