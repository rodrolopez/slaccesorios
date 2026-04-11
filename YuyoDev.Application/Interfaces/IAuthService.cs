namespace YuyoDev.Application.Interfaces;

using YuyoDev.Application.DTOs; // Asegurate de que el namespace coincida con donde tenés LoginRequest
using YuyoDev.Domain.Shared;

public interface IAuthService
{
    Task<Result<string>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<Result<string>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}