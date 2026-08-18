using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
