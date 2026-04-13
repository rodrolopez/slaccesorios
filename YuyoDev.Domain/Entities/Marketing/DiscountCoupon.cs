namespace YuyoDev.Domain.Entities.Marketing;

public class DiscountCoupon
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    // Ej: "10OFF", "CYBERMONDAY"
    public string Code { get; private set; } = string.Empty;
    public decimal DiscountPercentage { get; private set; }

    public DateTime ExpiryDate { get; private set; }
    public bool IsActive { get; private set; } = true;

    public int MaxUses { get; private set; } // Límite total de veces que se puede usar
    public int CurrentUses { get; private set; } = 0;

    protected DiscountCoupon() { }

    public static DiscountCoupon Create(string code, decimal discountPercentage, DateTime expiryDate, int maxUses)
    {
        return new DiscountCoupon
        {
            Code = code.ToUpper(),
            DiscountPercentage = discountPercentage,
            ExpiryDate = expiryDate,
            MaxUses = maxUses
        };
    }

    public void IncrementUse()
    {
        CurrentUses++;
        if (CurrentUses >= MaxUses) IsActive = false; // Se apaga solo si llega al límite
    }
}