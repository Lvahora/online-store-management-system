using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OnlineStore.Contracts.Orders;
using Xunit;

namespace OnlineStore.IntegrationTests;

public class OrderIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ValidRequest_ReturnsOk()
    {
        var request = new CreateOrderRequest(
            Guid.NewGuid(),
            new List<OrderItemRequest>
            {
                new(Guid.Parse("11111111-1111-1111-1111-111111111111"), 1)
            });

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var order = await response.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(order);
        Assert.Equal(50000m, order!.TotalPrice);
    }

    [Fact]
    public async Task CreateOrder_InsufficientStock_ReturnsBadRequest()
    {
        var request = new CreateOrderRequest(
            Guid.NewGuid(),
            new List<OrderItemRequest>
            {
                new(Guid.Parse("33333333-3333-3333-3333-333333333333"), 10)
            });

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
