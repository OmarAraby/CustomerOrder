using Autofac;
using Autofac.Integration.WebApi;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Application.Services;
using CustomerOrder.Core.Interfaces;
using CustomerOrder.Infrastructure.Persistence;
using CustomerOrder.Infrastructure.Persistence.Context;
using CustomerOrder.Infrastructure.Repositories;
using System.Reflection;
using System.Web.Http;

namespace CustomerOrder.Api
{

    // our dependency resolver is set up in AutofacConfig.Register,
    public static class AutofacConfig
    {
        public static void Register(HttpConfiguration config)
        {

            PersistenceBootstrapper.Configure();

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // --- Persistence 
            // instance per request == addscoped in .net core

            builder.RegisterType<AppDbContext>()
                   .AsSelf()  //   Register the AppDbContext as itself  
                   .InstancePerRequest();

            builder.RegisterType<CustomerRepository>()
                   .As<ICustomerRepository>()
                   .InstancePerRequest();

            builder.RegisterType<OrderRepository>()
                   .As<IOrderRepository>()
                   .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerRequest();

            // --- Application services
            builder.RegisterType<CustomerService>()
                   .As<ICustomerService>()
                   .InstancePerRequest();


            // --- Identity + JWT

            config.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}
