using Microsoft.AspNetCore.Identity;

namespace YuyoDev.Domain.Entities;

// Heredamos de IdentityUser para tener Email, PasswordHash, etc.
// Pero la personalizamos para tu marca.
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}