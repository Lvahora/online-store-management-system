namespace OnlineStore.Contracts.Orders;

public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemRequest> Items
);

public record OrderItemRequest(
    Guid ProductId,
    int Quantity
);
