namespace YuyoDev.WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Shared;

public class InventoryController : BaseApiController
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpPost("adjust")]
    public async Task<IActionResult> AdjustStock([FromBody] AdjustStockDto request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Quantity > 0)
            {
                await _inventoryService.AddStockAsync(request.ProductVariantId, request.Quantity, 0, request.Notes, cancellationToken);
            }
            else if (request.Quantity < 0)
            {
                await _inventoryService.ReduceStockAsync(request.ProductVariantId, Math.Abs(request.Quantity), request.Notes, cancellationToken);
            }

            return Ok(Result<string>.Success("Stock ajustado correctamente."));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }
}

public class AdjustStockDto
{
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public string Notes { get; set; } = string.Empty;
}