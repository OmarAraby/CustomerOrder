using System.Data.Entity;
using CustomerOrder.Infrastructure.Persistence.Context;

namespace CustomerOrder.Infrastructure.Persistence
{

    public static class PersistenceBootstrapper
    {

        public static void Configure()
        {
            Database.SetInitializer<AppDbContext>(null);
        }
    }
}
