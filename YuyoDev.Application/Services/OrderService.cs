namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs.Orders;
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Shared;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> CreateOrderAsync(CreateOrderDto request, CancellationToken cancellationToken)
    {
        if (!request.Items.Any()) return Result<Guid>.Failure("La orden debe tener al menos un ítem.");

        var order = Order.Create(request.UserId);

        foreach (var item in request.Items)
        {
            // Gracias al DDD, la orden maneja su propio estado y suma el total sola
            order.AddItem(item.ProductVariantId, item.Quantity, item.UnitPrice);
        }

        await _repository.AddOrderAsync(order, cancellationToken);
        return Result<Guid>.Success(order.Id);
    }
}