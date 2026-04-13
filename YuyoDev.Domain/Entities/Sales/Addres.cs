namespace YuyoDev.Domain.Entities.Sales;

public class Address
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    // Conectamos con el IdentityUser (suele ser string en ASP.NET Identity por defecto)
    public string UserId { get; private set; } = string.Empty;

    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public string Reference { get; private set; } = string.Empty; // Ej: "Casa con rejas negras"

    public bool IsDefault { get; private set; }

    protected Address() { }

    public static Address Create(string userId, string street, string city, string state, string zipCode, string reference)
    {
        return new Address
        {
            UserId = userId,
            Street = street,
            City = city,
            State = state,
            ZipCode = zipCode,
            Reference = reference,
            IsDefault = false
        };
    }
}