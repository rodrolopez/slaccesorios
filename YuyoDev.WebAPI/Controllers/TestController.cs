using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YuyoDev.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("publico")]
    public IActionResult GetPublico()
    {
        return Ok("Este mensaje lo ve cualquiera, hasta el que no tiene token.");
    }

    [Authorize] // <--- ESTE ES EL CANDADO
    [HttpGet("privado")]
    public IActionResult GetPrivado()
    {
        return Ok("¡Felicidades Yuyo! Si ves esto es porque tu Token JWT es válido y sos un usuario autenticado.");
    }
}