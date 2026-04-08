using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YuyoDev.Domain.Entities; // Importante para que encuentre tu ApplicationUser

namespace YuyoDev.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Esto es un "Yuyo Tip": PostgreSQL prefiere snake_case.
        // Por ahora lo dejamos estándar, pero aquí podrías renombrar las tablas de Identity.
    }
}