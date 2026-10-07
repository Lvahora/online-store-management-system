using Microsoft.Extensions.DependencyInjection;

namespace OnlineStore.Orders;

public static class OrdersServiceCollectionExtensions
{
    public static IServiceCollection AddOrdersModule(this IServiceCollection services)
    {
        services.AddSingleton<OrderService>();
        return services;
    }
}
