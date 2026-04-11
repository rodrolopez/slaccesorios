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

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var result = await _productService.GetAllAsync(pageNumber, pageSize, searchTerm, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto request, CancellationToken cancellationToken)
    {
        var result = await _productService.CreateProductAsync(request, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetProductById), new { id = result.Value }, result);
        }
        return HandleResult(result);
    }
}