using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuyoDev.Domain.Entities;

namespace YuyoDev.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SupAdmin")] // Solo vos podés gestionar la base de usuarios
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: api/User (Listar todos)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userManager.Users
            .Select(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.CreatedAt })
            .ToListAsync();
        return Ok(users);
    }

    // GET: api/User/{id} (Obtener uno solo)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound("Usuario no encontrado.");

        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email, user.CreatedAt });
    }

    // PUT: api/User/{id} (Actualizar datos)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok("Usuario actualizado con éxito.");
    }

    // DELETE: api/User/{id} (Eliminar usuario)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        // Evitar que el SupAdmin se borre a sí mismo por error
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser?.Id == id) return BadRequest("No puedes eliminar tu propia cuenta de SupAdmin.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok($"Usuario {user.Email} eliminado correctamente.");
    }
}

// DTO simple para la actualización
public record UpdateUserRequest(string FirstName, string LastName);