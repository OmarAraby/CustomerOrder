using CustomerOrder.Application.Dtos.Auth;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Infrastructure.Persistence.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerOrder.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {

        private readonly AppDbContext _context;
        public IdentityService(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<AuthenticatedUser> ValidateCredentialsAsync(string userName, string password)
        {
            //throw new System.NotImplementedException();


            // for now after investigaton best is use using 
            using (var store = new UserStore<ApplicationUser>(_context))
            using (var userManager = new UserManager<ApplicationUser>(store))
            {
                var user = await userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return null;
                }

                if (!await userManager.CheckPasswordAsync(user, password))
                {
                    return null;
                }

                var roles = await userManager.GetRolesAsync(user.Id);

                return new AuthenticatedUser
                {
                    UserName = user.UserName,
                    Roles = roles.ToList()
                };
            }

        }
    }
}
