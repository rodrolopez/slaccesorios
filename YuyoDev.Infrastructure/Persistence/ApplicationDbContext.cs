using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YuyoDev.Domain.Entities;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Interfaces;

namespace YuyoDev.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ITenantService _tenantService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<AuditLog> AuditLogs { get; set; }

    // UNIFICAMOS EL MÉTODO ACÁ
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 1. Configuramos la tabla de Auditoría para que sea eficiente (lo que ya tenías)
        builder.Entity<AuditLog>(entity =>
        {
            entity.Property(a => a.Action).HasMaxLength(100).IsRequired();
            entity.Property(a => a.EntityName).HasMaxLength(100).IsRequired();
        });

        // 2. MAGIA MULTITENANT: Filtro global para que no se mezclen los clientes
        builder.Entity<AuditLog>().HasQueryFilter(a => a.TenantId == _tenantService.GetTenant());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // Asignación automática del TenantId antes de guardar en la base de datos
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
        {
            entry.Entity.TenantId = _tenantService.GetTenant();
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}