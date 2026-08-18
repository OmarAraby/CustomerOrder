using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate,CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync( Expression<Func<T, bool>> predicate,  CancellationToken cancellationToken = default);
        void Add(T entity);
        void Remove(T entity);
    }
}
