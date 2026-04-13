namespace YuyoDev.Domain.Entities.Sales;

using YuyoDev.Domain.Entities;

public class Invoice
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid OrderId { get; private set; }
    public virtual Order? Order { get; private set; }

    public string InvoiceNumber { get; private set; } = string.Empty; // Ej: "FC-A-0001-00001234"
    public string TaxId { get; private set; } = string.Empty; // CUIT o DNI del cliente
    public string CustomerName { get; private set; } = string.Empty; // Nombre o Razón Social

    public decimal TotalAmount { get; private set; }

    public string? PdfUrl { get; private set; } // Link al PDF guardado en la nube (S3, Cloudinary)

    public DateTime IssuedAt { get; private set; } = DateTime.UtcNow;

    protected Invoice() { }

    public static Invoice Create(Guid orderId, string invoiceNumber, string taxId, string customerName, decimal totalAmount, string? pdfUrl)
    {
        return new Invoice
        {
            OrderId = orderId,
            InvoiceNumber = invoiceNumber,
            TaxId = taxId,
            CustomerName = customerName,
            TotalAmount = totalAmount,
            PdfUrl = pdfUrl
        };
    }
}