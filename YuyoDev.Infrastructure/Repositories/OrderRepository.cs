namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;
using YuyoDev.Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    public OrderRepository(ApplicationDbContext context) => _context = context;

    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}