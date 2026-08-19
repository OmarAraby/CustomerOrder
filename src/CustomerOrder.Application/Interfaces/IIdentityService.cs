using CustomerOrder.Application.Dtos.Auth;
using System.Threading.Tasks;

namespace CustomerOrder.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticatedUser> ValidateCredentialsAsync(string userName, string password);
    }
}
