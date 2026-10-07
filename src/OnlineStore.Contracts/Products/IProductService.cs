namespace OnlineStore.Contracts.Products;

public interface IProductService
{
    Task<ProductDto?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<bool> CheckStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
}
