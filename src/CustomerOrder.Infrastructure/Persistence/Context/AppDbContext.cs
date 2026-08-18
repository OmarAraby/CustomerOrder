using CustomerOrder.Core.Entities;
using CustomerOrder.Infrastructure.Identity;
using CustomerOrder.Infrastructure.Persistence.Configurations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CustomerOrder.Infrastructure.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext() : base("name=CustomerOrderDb")
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
        }
    }
}
