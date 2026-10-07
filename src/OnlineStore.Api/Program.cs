using OnlineStore.Catalog;
using OnlineStore.Orders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCatalogModule();
builder.Services.AddOrdersModule();

var app = builder.Build();

app.MapGet("/api/products/{id}", async (Guid id, IProductService productService, CancellationToken ct) =>
{
    var product = await productService.GetProductAsync(id, ct);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapPost("/api/orders", async (CreateOrderRequest request, OrderService orderService, CancellationToken ct) =>
{
    var order = await orderService.CreateOrderAsync(request, ct);
    return order is null ? Results.BadRequest("Не удалось создать заказ") : Results.Ok(order);
});

app.Run();
