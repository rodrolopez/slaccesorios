namespace YuyoDev.WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs.Products;

public class ProductsController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto request, CancellationToken cancellationToken)
    {
        var result = await _productService.CreateProductAsync(request, cancellationToken);

        // Si sale bien, devolvemos 201 Created. Si falla, el BaseApiController maneja el 400.
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetProductById), new { id = result.Value }, result);
        }
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}