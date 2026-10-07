using Microsoft.Extensions.Logging.Abstractions;
using OnlineStore.Catalog;
using Xunit;

namespace OnlineStore.UnitTests.Catalog;

public class ProductServiceTests
{
    private readonly ProductService _service = new(NullLogger<ProductService>.Instance);

    [Fact]
    public async Task GetProduct_ExistingProduct_ReturnsProduct()
    {
        var productId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var product = await _service.GetProductAsync(productId);
        Assert.NotNull(product);
        Assert.Equal("Смартфон", product!.Name);
    }

    [Fact]
    public async Task GetProduct_NonExistingProduct_ReturnsNull()
    {
        var product = await _service.GetProductAsync(Guid.NewGuid());
        Assert.Null(product);
    }

    [Fact]
    public async Task CheckStock_SufficientStock_ReturnsTrue()
    {
        var productId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var result = await _service.CheckStockAsync(productId, 5);
        Assert.True(result);
    }

    [Fact]
    public async Task CheckStock_InsufficientStock_ReturnsFalse()
    {
        var productId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var result = await _service.CheckStockAsync(productId, 1);
        Assert.False(result);
    }
}
