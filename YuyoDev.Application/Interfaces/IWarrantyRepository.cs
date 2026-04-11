namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities;

public interface IWarrantyRepository
{
    Task AddTicketAsync(WarrantyTicket ticket, CancellationToken cancellationToken);
    Task<WarrantyTicket?> GetTicketByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateTicketAsync(WarrantyTicket ticket, CancellationToken cancellationToken);

    // Validamos que el producto que nos reclaman realmente exista en una orden
    Task<bool> OrderItemExistsAsync(Guid orderItemId, CancellationToken cancellationToken);
}