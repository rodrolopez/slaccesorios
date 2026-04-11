namespace YuyoDev.WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs.Warranties;

public class WarrantiesController : BaseApiController
{
    private readonly IWarrantyService _warrantyService;

    public WarrantiesController(IWarrantyService warrantyService)
    {
        _warrantyService = warrantyService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateWarrantyTicketDto request, CancellationToken cancellationToken)
    {
        var result = await _warrantyService.CreateTicketAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetTicketById), new { id = result.Value }, result);
        }

        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTicketById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _warrantyService.GetTicketByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}