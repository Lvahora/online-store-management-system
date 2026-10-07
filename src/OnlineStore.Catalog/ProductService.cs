using Microsoft.Extensions.Logging;
using OnlineStore.Contracts.Products;

namespace OnlineStore.Catalog;

public class ProductService : IProductService
{
    private readonly List<Product> _products;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ILogger<ProductService> logger)
    {
        _logger = logger;
        _products = new List<Product>
        {
            new Product { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Смартфон", Category = "Электроника", Price = 50000, StockQuantity = 10 },
            new Product { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Ноутбук", Category = "Электроника", Price = 80000, StockQuantity = 5 },
            new Product { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Наушники", Category = "Аксессуары", Price = 5000, StockQuantity = 0 }
        };
    }

    public Task<ProductDto?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Запрос товара {ProductId}", productId);
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            _logger.LogWarning("Товар {ProductId} не найден", productId);
            return Task.FromResult<ProductDto?>(null);
        }

        return Task.FromResult<ProductDto?>(new ProductDto(
            product.Id, product.Name, product.Category, product.Price, product.StockQuantity));
    }

    public Task<bool> CheckStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Проверка остатка товара {ProductId}, количество {Quantity}", productId, quantity);
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            _logger.LogWarning("Товар {ProductId} не найден при проверке остатка", productId);
            return Task.FromResult(false);
        }

        var hasStock = product.StockQuantity >= quantity;
        _logger.LogInformation("Товар {ProductId}: остаток {Stock}, запрошено {Quantity}, результат {HasStock}",
            productId, product.StockQuantity, quantity, hasStock);
        return Task.FromResult(hasStock);
    }
}
