using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CustomerOrder.Application.Dtos.Orders;

namespace CustomerOrder.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IReadOnlyList<OrderSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<OrderDetailsDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<OrderSummaryDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default);

        Task UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}