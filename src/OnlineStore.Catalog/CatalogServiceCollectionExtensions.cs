using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Contracts.Products;

namespace OnlineStore.Catalog;

public static class CatalogServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        services.AddSingleton<IProductService, ProductService>();
        return services;
    }
}
