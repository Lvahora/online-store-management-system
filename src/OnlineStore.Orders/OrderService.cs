using Microsoft.Extensions.Logging;
using OnlineStore.Contracts.Orders;
using OnlineStore.Contracts.Products;

namespace OnlineStore.Orders;

public class OrderService
{
    private readonly List<Order> _inMemoryOrders = new();
    private readonly IProductService _productService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IProductService productService, ILogger<OrderService> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    public async Task<OrderDto?> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            _logger.LogError("Запрос на создание заказа не может быть null");
            return null;
        }

        if (request.Items == null || request.Items.Count == 0)
        {
            _logger.LogWarning("Заказ должен содержать хотя бы один товар");
            return null;
        }

        if (request.CustomerId == Guid.Empty)
        {
            _logger.LogWarning("Невалидный ID клиента: {CustomerId}", request.CustomerId);
            return null;
        }

        _logger.LogInformation("Создание заказа для клиента {CustomerId}", request.CustomerId);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CreatedAt = DateTime.UtcNow
        };

        decimal totalPrice = 0;

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
            {
                _logger.LogWarning("Некорректное количество товара {ProductId}: {Quantity}", item.ProductId, item.Quantity);
                return null;
            }

            var product = await _productService.GetProductAsync(item.ProductId, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning("Товар {ProductId} не найден", item.ProductId);
                return null;
            }

            var hasStock = await _productService.CheckStockAsync(item.ProductId, item.Quantity, cancellationToken);
            if (!hasStock)
            {
                _logger.LogWarning("Недостаточно товара {ProductId} на складе. Запрошено: {Quantity}, доступно: {Stock}",
                    item.ProductId, item.Quantity, product.StockQuantity);
                return null;
            }

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                Price = product.Price
            });

            totalPrice += product.Price * item.Quantity;
        }

        order.TotalPrice = totalPrice;
        order.Status = "Created";
        _inMemoryOrders.Add(order);

        _logger.LogInformation("Заказ {OrderId} создан на сумму {TotalPrice}", order.Id, order.TotalPrice);

        return new OrderDto(order.Id, order.CustomerId, order.CreatedAt, order.TotalPrice, order.Status);
    }
}
