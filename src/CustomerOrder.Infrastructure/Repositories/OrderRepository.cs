using CustomerOrder.Core.Entities;
using CustomerOrder.Core.Interfaces;
using CustomerOrder.Infrastructure.Persistence.Context;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Order> GetWithCustomersAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Entities
                .Include(o => o.Customers)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }
        public async Task<bool> OrderNumberExistsAsync(string orderNumber, int? excludeOrderId = null, CancellationToken cancellationToken = default)
        {
            var query = Entities.Where(o => o.OrderNumber == orderNumber);

            if (excludeOrderId.HasValue)
            {
                query = query.Where(o => o.Id != excludeOrderId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }
    }
}
