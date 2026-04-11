namespace YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs.Orders;
using YuyoDev.Domain.Shared;

public interface IOrderService
{
    Task<Result<Guid>> CreateOrderAsync(CreateOrderDto request, CancellationToken cancellationToken);
}