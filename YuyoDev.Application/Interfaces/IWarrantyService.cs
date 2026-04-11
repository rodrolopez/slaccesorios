namespace YuyoDev.Application.Interfaces;

using YuyoDev.Application.DTOs.Warranties;
using YuyoDev.Domain.Shared;

public interface IWarrantyService
{
    Task<Result<Guid>> CreateTicketAsync(CreateWarrantyTicketDto request, CancellationToken cancellationToken);
    Task<Result<WarrantyTicketDto>> GetTicketByIdAsync(Guid id, CancellationToken cancellationToken);
}