using CustomerOrder.Core.Interfaces;
using CustomerOrder.Infrastructure.Persistence.Context;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(AppDbContext context, ICustomerRepository customers, IOrderRepository orders)
        {
            _context = context;
            Customers = customers;
            Orders = orders;
        }
  

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
