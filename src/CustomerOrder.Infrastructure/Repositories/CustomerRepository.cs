using CustomerOrder.Core.Entities;
using CustomerOrder.Core.Interfaces;
using CustomerOrder.Infrastructure.Persistence.Context;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Infrastructure.Repositories
{
    public class CustomerRepository :GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Customer> GetWithOrdersAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Entities.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<bool> EmailExistsAsync(string email, int? excludeCustomerId = null, CancellationToken cancellationToken = default)
        {
            var query = Entities.Where(c => c.Email == email);
            if (excludeCustomerId.HasValue)
            {
                query = query.Where(c => c.Id != excludeCustomerId.Value);
            }
            return await query.AnyAsync(cancellationToken);
        }
    }
}
