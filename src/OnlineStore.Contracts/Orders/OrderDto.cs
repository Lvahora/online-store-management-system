namespace OnlineStore.Contracts.Orders;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    DateTime CreatedAt,
    decimal TotalPrice,
    string Status
);

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Price
);
