using CustomerOrder.Application.Dtos.Customers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CustomerDetailsDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int id, UpdateCustomerDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
