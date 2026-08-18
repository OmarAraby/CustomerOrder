using CustomerOrder.Core.Interfaces;
using CustomerOrder.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected AppDbContext Context { get; }

        protected DbSet<T> Entities
        {
            get { return Context.Set<T>(); }
        }

        public GenericRepository(AppDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Context = context;
        }

        public virtual Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Entities.FindAsync(cancellationToken, id);
        }

        public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await Entities
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Entities
                    .AsNoTracking()
                    .Where(predicate)
                    .ToListAsync(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync( Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Entities.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<bool> AnyAsync(  Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Entities.AnyAsync(predicate, cancellationToken);
        }

        public void Add(T entity)
        {
            Entities.Add(entity);
        }

        public void Remove(T entity)
        {
            Entities.Remove(entity);
        }
    }
}
