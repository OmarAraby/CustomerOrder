namespace CustomerOrder.Infrastructure.Persistence.Migrations
{
    using CustomerOrder.Infrastructure.Identity;
    using CustomerOrder.Infrastructure.Persistence.Context;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppDbContext context)
        {
            IdentitySeeder.Seed(context);
        }
    }
}