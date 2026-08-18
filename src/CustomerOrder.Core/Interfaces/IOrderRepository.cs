using CustomerOrder.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetWithCustomersAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> OrderNumberExistsAsync(string orderNumber, int? excludeOrderId = null, CancellationToken cancellationToken = default);
    }
}
