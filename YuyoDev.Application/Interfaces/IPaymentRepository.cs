namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.Sales;

public interface IPaymentRepository
{
    Task AddTransactionAsync(PaymentTransaction transaction, CancellationToken cancellationToken);
    Task UpdateTransactionAsync(PaymentTransaction transaction, CancellationToken cancellationToken);
    Task<PaymentTransaction?> GetTransactionByExternalReferenceAsync(string externalReference, CancellationToken cancellationToken);

    Task AddInvoiceAsync(Invoice invoice, CancellationToken cancellationToken);
}