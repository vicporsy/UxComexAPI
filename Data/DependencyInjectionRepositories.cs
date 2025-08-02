using Microsoft.Extensions.DependencyInjection;
using Vicporsy.Data.Context;
using Vicporsy.Data.Repositories;
using Vicporsy.Domain.Interface;
using Vicporsy.Domain.Interface.Repositories;

namespace Vicporsy.Data
{
    public static class DependencyInjectionRepositories
    {
        public static IServiceCollection AddDependencyInjectionRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IClientRepository, ClientRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IOrderItemRepository, OrderItemRepository>();

            return services;
        }
    }
}
