
using Microsoft.Extensions.DependencyInjection;
using Vicporsy.Application.Mapper;
using Vicporsy.Application.Services;
using Vicporsy.Domain.Interface.Services;

namespace Vicporsy.Application
{
    public static class DependencyInjectionServices
    {
        public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddSingleton<IClientService, ClientService>();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<IOrderItemService, OrderItemService>();
            services.AddSingleton<IProductService, ProductService>();
            return services;
        }
    }
}
