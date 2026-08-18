using CustomerOrder.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Core.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetWithOrdersAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync( string email, int? excludeCustomerId = null,CancellationToken cancellationToken = default);

    }
}
