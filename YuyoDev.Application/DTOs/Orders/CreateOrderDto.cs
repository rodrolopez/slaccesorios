namespace YuyoDev.Application.DTOs.Orders;

public class CreateOrderDto
{
    public string UserId { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}