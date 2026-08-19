using CustomerOrder.Application.Dtos.Orders;
using CustomerOrder.Application.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Application.Services
{
    public class OrderService : IOrderService
    {
        public Task<OrderSummaryDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<OrderSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<OrderDetailsDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
