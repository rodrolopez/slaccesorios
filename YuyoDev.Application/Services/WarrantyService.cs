namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs.Warranties;
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Shared;

public class WarrantyService : IWarrantyService
{
    private readonly IWarrantyRepository _repository;

    public WarrantyService(IWarrantyRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> CreateTicketAsync(CreateWarrantyTicketDto request, CancellationToken cancellationToken)
    {
        // 1. Verificamos que no nos estén inventando un ítem que nunca se vendió
        var itemExists = await _repository.OrderItemExistsAsync(request.OrderItemId, cancellationToken);
        if (!itemExists) return Result<Guid>.Failure("El ítem de la orden especificado no existe.");

        // 2. Usamos el método de nuestro Dominio Rico para crear el ticket de forma segura
        var ticket = WarrantyTicket.Create(request.OrderItemId, request.IssueDescription);

        await _repository.AddTicketAsync(ticket, cancellationToken);

        return Result<Guid>.Success(ticket.Id);
    }

    public async Task<Result<WarrantyTicketDto>> GetTicketByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetTicketByIdAsync(id, cancellationToken);

        if (ticket is null) return Result<WarrantyTicketDto>.Failure("Ticket de garantía no encontrado.");

        var dto = new WarrantyTicketDto
        {
            Id = ticket.Id,
            OrderItemId = ticket.OrderItemId,
            IssueDescription = ticket.IssueDescription,
            ResolutionNotes = ticket.ResolutionNotes,
            Status = ticket.Status.ToString(), // Convertimos el Enum a string para el Frontend
            CreatedAt = ticket.CreatedAt,
            ResolvedAt = ticket.ResolvedAt
        };

        return Result<WarrantyTicketDto>.Success(dto);
    }
}