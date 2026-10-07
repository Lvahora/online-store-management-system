using Microsoft.Extensions.Logging.Abstractions;
using OnlineStore.Orders;
using OnlineStore.Catalog;
using OnlineStore.Contracts.Orders;
using OnlineStore.Contracts.Products;
using Xunit;

namespace OnlineStore.UnitTests.Orders;

public class OrderServiceTests
{
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        IProductService productService = new ProductService(NullLogger<ProductService>.Instance);
        _service = new OrderService(productService, NullLogger<OrderService>.Instance);
    }

    [Fact]
    public async Task CreateOrder_ValidRequest_ReturnsOrder()
    {
        var request = new CreateOrderRequest(
            Guid.NewGuid(),
            new List<OrderItemRequest>
            {
                new(Guid.Parse("11111111-1111-1111-1111-111111111111"), 2)
            });

        var order = await _service.CreateOrderAsync(request);

        Assert.NotNull(order);
        Assert.Equal(100000m, order!.TotalPrice);
        Assert.Equal("Created", order.Status);
    }

    [Fact]
    public async Task CreateOrder_InsufficientStock_ReturnsNull()
    {
        var request = new CreateOrderRequest(
            Guid.NewGuid(),
            new List<OrderItemRequest>
            {
                new(Guid.Parse("33333333-3333-3333-3333-333333333333"), 5)
            });

        var order = await _service.CreateOrderAsync(request);

        Assert.Null(order);
    }

    [Fact]
    public async Task CreateOrder_NonExistingProduct_ReturnsNull()
    {
        var request = new CreateOrderRequest(
            Guid.NewGuid(),
            new List<OrderItemRequest>
            {
                new(Guid.NewGuid(), 1)
            });

        var order = await _service.CreateOrderAsync(request);

        Assert.Null(order);
    }
}
