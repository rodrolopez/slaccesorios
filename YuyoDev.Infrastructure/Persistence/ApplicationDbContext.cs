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

    // DbSets de Infraestructura del Boilerplate
    public DbSet<AuditLog> AuditLogs { get; set; }

    // DbSets de la Fase 1: Catálogo E-commerce
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<WarrantyTicket> WarrantyTickets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Fundamental mantenerlo arriba para Identity

        var currentTenantId = _tenantService.GetTenant();

        // 1. Configuraciones del Boilerplate
        builder.Entity<AuditLog>(entity =>
        {
            entity.Property(a => a.Action).HasMaxLength(100).IsRequired();
            entity.Property(a => a.EntityName).HasMaxLength(100).IsRequired();
        });

        // 2. MAGIA MULTITENANT: Filtros globales para todo el dominio
        builder.Entity<AuditLog>().HasQueryFilter(a => a.TenantId == currentTenantId);
        builder.Entity<Category>().HasQueryFilter(c => c.TenantId == currentTenantId);
        builder.Entity<Brand>().HasQueryFilter(b => b.TenantId == currentTenantId);
        builder.Entity<Product>().HasQueryFilter(p => p.TenantId == currentTenantId);
        builder.Entity<ProductVariant>().HasQueryFilter(pv => pv.TenantId == currentTenantId);

        // 3. Reglas de negocio y relaciones (Fluent API)
        builder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();

            // Relaciones: Evitamos borrar categorías/marcas si tienen productos asociados
            entity.HasOne(p => p.Category)
                  .WithMany()
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Brand)
                  .WithMany()
                  .HasForeignKey(p => p.BrandId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sku).HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.Sku).IsUnique(); // El SKU es único a nivel tabla

            // Precisión PostgreSQL para manejar la plata
            entity.Property(e => e.Price).HasColumnType("numeric(18,2)");

            // Cascada: Si se elimina el producto padre, se van sus variantes
            entity.HasOne(v => v.Product)
                  .WithMany(p => p.Variants)
                  .HasForeignKey(v => v.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Filtros Multitenant para Órdenes
        builder.Entity<Order>().HasQueryFilter(o => o.TenantId == currentTenantId);
        builder.Entity<OrderItem>().HasQueryFilter(oi => oi.TenantId == currentTenantId);

        // Fluent API para Órdenes
        builder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasColumnType("numeric(18,2)");
            // UserId se mapeará con IdentityUser, lo dejamos como string requerido por ahora
            entity.Property(e => e.UserId).IsRequired();
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("numeric(18,2)");

            // Relación con la orden (Cascada: si borro la orden, se borran sus items)
            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con la variante del producto
            entity.HasOne(oi => oi.ProductVariant)
                .WithMany()
                .HasForeignKey(oi => oi.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        // Filtro Multitenant para Garantías
        builder.Entity<WarrantyTicket>().HasQueryFilter(w => w.TenantId == currentTenantId);

        // Fluent API para Garantías
        builder.Entity<WarrantyTicket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IssueDescription).IsRequired().HasMaxLength(1000);

            // Relación con el ítem comprado (Restrict para no borrar el ítem si hay un ticket abierto)
            entity.HasOne(w => w.OrderItem)
                .WithMany()
                .HasForeignKey(w => w.OrderItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // Asignación automática del TenantId antes de guardar
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
        {
            if (string.IsNullOrEmpty(entry.Entity.TenantId))
            {
                entry.Entity.TenantId = _tenantService.GetTenant();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}