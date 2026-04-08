using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YuyoDev.Application.DTOs;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;

namespace YuyoDev.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Buscamos el usuario por email
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return Unauthorized("Credenciales inválidas");

        // 2. Verificamos la contraseña
        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!result) return Unauthorized("Credenciales inválidas");

        // 3. ¡NUEVO! Buscamos qué roles tiene este usuario en la base de datos
        var roles = await _userManager.GetRolesAsync(user);

        // 4. Si todo está bien, generamos el Token inyectando los roles
        return Ok(new {
            token = _tokenService.CreateToken(user, roles),
            email = user.Email
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // ¡NUEVO! Le asignamos el rol básico por defecto a todo usuario nuevo
        await _userManager.AddToRoleAsync(user, "User");

        return Ok("¡Usuario de Yuyo Dev creado con éxito y rol asignado!");
    }
}