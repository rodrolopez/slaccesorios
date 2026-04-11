namespace YuyoDev.Application.Services;

using Microsoft.AspNetCore.Identity;
using YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs; // O donde estén LoginRequest y RegisterRequest
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Shared;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<Result<string>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        // Verificamos si el email ya existe
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result<string>.Failure("El correo electrónico ya está registrado.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName, // Asumiendo que tenés estas propiedades en el DTO y ApplicationUser
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Failure($"Error al crear el usuario: {errors}");
        }

        // Por defecto le asignamos un rol de cliente, asumiendo que tu SeedRolesAsync ya los creó
        await _userManager.AddToRoleAsync(user, "Client");

        return Result<string>.Success("Usuario registrado exitosamente.");
    }

    public async Task<Result<string>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<string>.Failure("Credenciales inválidas.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return Result<string>.Failure("Credenciales inválidas.");
        }

        // Si todo está bien, generamos el JWT
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);

        return Result<string>.Success(token);
    }
}