namespace YuyoDev.WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using YuyoDev.Domain.Shared; // O donde esté tu Result<T>

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    // Método helper para estandarizar el retorno del Result<T>
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value != null ? Ok(result) : NotFound(result);
        }

        // Si falla, devolvemos un 400 Bad Request con el Result que contiene el error
        return BadRequest(result);
    }
}