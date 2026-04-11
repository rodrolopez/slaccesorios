namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;
using YuyoDev.Infrastructure.Persistence;

public class WarrantyRepository : IWarrantyRepository
{
    private readonly ApplicationDbContext _context;

    public WarrantyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddTicketAsync(WarrantyTicket ticket, CancellationToken cancellationToken)
    {
        _context.WarrantyTickets.Add(ticket);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<WarrantyTicket?> GetTicketByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // Traemos el ticket e incluimos los datos del ítem comprado para tener contexto
        return await _context.WarrantyTickets
            .Include(w => w.OrderItem)
            .ThenInclude(oi => oi.ProductVariant)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task UpdateTicketAsync(WarrantyTicket ticket, CancellationToken cancellationToken)
    {
        _context.WarrantyTickets.Update(ticket);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> OrderItemExistsAsync(Guid orderItemId, CancellationToken cancellationToken)
    {
        return _context.OrderItems.AnyAsync(oi => oi.Id == orderItemId, cancellationToken);
    }
}