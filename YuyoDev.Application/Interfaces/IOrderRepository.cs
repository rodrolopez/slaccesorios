namespace YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateOrderAsync(Order order, CancellationToken cancellationToken);
}