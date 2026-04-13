namespace YuyoDev.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Entities.Logistics;
using YuyoDev.Domain.Entities.CMS;
using YuyoDev.Domain.Entities.Catalog;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Abstractions;
using YuyoDev.Domain.Entities.Sales;
using YuyoDev.Domain.Entities.Shipping;
using YuyoDev.Domain.Entities.Marketing;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ITenantService _tenantService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
    }

    // Boilerplate
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // Catálogo y E-commerce
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

    // Órdenes y Logística
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<WarrantyTicket> WarrantyTickets => Set<WarrantyTicket>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    // CMS
    public DbSet<StoreConfiguration> StoreConfigurations => Set<StoreConfiguration>();
    // --- BLOQUE 4: CLIENTES Y COMPRAS ---
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();

    // --- BLOQUE 5: PAGOS Y FACTURACIÓN ---
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    // --- BLOQUE 6: ENVÍOS ---
    public DbSet<ShippingMethod> ShippingMethods => Set<ShippingMethod>();
    public DbSet<ShippingZone> ShippingZones => Set<ShippingZone>();
    // --- BLOQUE 7: MARKETING ---
    public DbSet<DiscountCoupon> DiscountCoupons => Set<DiscountCoupon>();
    public DbSet<ProductReview> ProductReviews => Set<ProductReview>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var currentTenantId = _tenantService.GetTenant();

        // Boilerplate
        builder.Entity<AuditLog>(entity =>
        {
            entity.Property(a => a.Action).HasMaxLength(100).IsRequired();
            entity.Property(a => a.EntityName).HasMaxLength(100).IsRequired();
        });

        // --- CATÁLOGO ---
        builder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Category>().HasIndex(c => c.Slug).IsUnique();

        builder.Entity<Product>()
            .HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
        });

        builder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SKU).HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.SKU).IsUnique();
            entity.Property(e => e.Price).HasColumnType("numeric(18,2)");
            entity.Property(e => e.CostPrice).HasColumnType("numeric(18,2)");
            entity.Property(e => e.SellingPrice).HasColumnType("numeric(18,2)");

            entity.HasOne(v => v.Product)
                  .WithMany(p => p.Variants)
                  .HasForeignKey(v => v.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<ProductVariant>().HasQueryFilter(pv => pv.TenantId == currentTenantId);

        // --- LOGÍSTICA ---
        builder.Entity<StockMovement>()
            .HasOne(sm => sm.ProductVariant)
            .WithMany()
            .HasForeignKey(sm => sm.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- ÓRDENES ---
        builder.Entity<Order>().HasQueryFilter(o => o.TenantId == currentTenantId);
        builder.Entity<OrderItem>().HasQueryFilter(oi => oi.TenantId == currentTenantId);

        builder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasColumnType("numeric(18,2)");
            entity.Property(e => e.UserId).IsRequired();
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("numeric(18,2)");

            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.ProductVariant)
                .WithMany()
                .HasForeignKey(oi => oi.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --- GARANTÍAS ---
        builder.Entity<WarrantyTicket>().HasQueryFilter(w => w.TenantId == currentTenantId);
        builder.Entity<WarrantyTicket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IssueDescription).IsRequired().HasMaxLength(1000);

            entity.HasOne(w => w.OrderItem)
                .WithMany()
                .HasForeignKey(w => w.OrderItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        // El Carrito y sus Items (Cascada: si borro el carrito, vuelan sus items)
        builder.Entity<ShoppingCart>()
            .HasMany(s => s.Items)
            .WithOne(i => i.ShoppingCart)
            .HasForeignKey(i => i.ShoppingCartId)
            .OnDelete(DeleteBehavior.Cascade);

        // El Item del carrito y el Producto
        builder.Entity<CartItem>()
            .HasOne(ci => ci.ProductVariant)
            .WithMany()
            .HasForeignKey(ci => ci.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Wishlist y el Producto (Cascada: si discontinúo/borro un producto, desaparece de los favoritos)
        builder.Entity<Wishlist>()
            .HasOne(w => w.Product)
            .WithMany()
            .HasForeignKey(w => w.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        // --- CONFIGURACIÓN PAGOS Y FACTURACIÓN (BLOQUE 5) ---

        // Una transacción pertenece a una Orden
        builder.Entity<PaymentTransaction>()
            .HasOne(pt => pt.Order)
            .WithMany()
            .HasForeignKey(pt => pt.OrderId)
            .OnDelete(DeleteBehavior.Restrict); // No borramos pagos por accidente

        // Una Factura pertenece a una Orden (Relación 1 a 1)
        builder.Entity<Invoice>()
            .HasOne(i => i.Order)
            .WithOne()
            .HasForeignKey<Invoice>(i => i.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- CONFIGURACIÓN ENVÍOS (BLOQUE 6) ---
        // Una Zona de envío pertenece a un Método específico
        builder.Entity<ShippingZone>()
            .HasOne(sz => sz.ShippingMethod)
            .WithMany()
            .HasForeignKey(sz => sz.ShippingMethodId)
            .OnDelete(DeleteBehavior.Cascade); // Si el admin borra el método "Moto", se borran sus zonas
        // --- CONFIGURACIÓN MARKETING (BLOQUE 7) ---
        // Una reseña pertenece a un Producto
        builder.Entity<ProductReview>()
            .HasOne(pr => pr.Product)
            .WithMany()
            .HasForeignKey(pr => pr.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // Si el admin borra el producto, vuelan sus reseñas
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
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