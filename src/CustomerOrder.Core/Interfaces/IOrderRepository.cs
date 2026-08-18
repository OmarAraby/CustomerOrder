using CustomerOrder.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Core.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetWithCustomersAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> OrderNumberExistsAsync(string orderNumber, int? excludeOrderId = null, CancellationToken cancellationToken = default);
    }
}
