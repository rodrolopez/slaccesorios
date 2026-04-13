namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.Sales;
using YuyoDev.Infrastructure.Persistence;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddTransactionAsync(PaymentTransaction transaction, CancellationToken cancellationToken)
    {
        _context.PaymentTransactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTransactionAsync(PaymentTransaction transaction, CancellationToken cancellationToken)
    {
        _context.PaymentTransactions.Update(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PaymentTransaction?> GetTransactionByExternalReferenceAsync(string externalReference, CancellationToken cancellationToken)
    {
        return await _context.PaymentTransactions
            .FirstOrDefaultAsync(pt => pt.ExternalReference == externalReference, cancellationToken);
    }

    public async Task AddInvoiceAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync(cancellationToken);
    }
}