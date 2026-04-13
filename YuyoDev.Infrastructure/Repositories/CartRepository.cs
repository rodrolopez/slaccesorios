namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.Sales;
using YuyoDev.Infrastructure.Persistence;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShoppingCart?> GetCartByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.ProductVariant)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public async Task<ShoppingCart?> GetCartBySessionIdAsync(string sessionId, CancellationToken cancellationToken)
    {
        return await _context.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.ProductVariant)
            .FirstOrDefaultAsync(c => c.SessionId == sessionId, cancellationToken);
    }

    public async Task AddCartAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        _context.ShoppingCarts.Add(cart);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCartAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        _context.ShoppingCarts.Update(cart);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCartAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        _context.ShoppingCarts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId, CancellationToken cancellationToken)
    {
        return await _context.CartItems.FindAsync(new object[] { cartItemId }, cancellationToken);
    }

    public async Task RemoveCartItemAsync(CartItem item, CancellationToken cancellationToken)
    {
        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }
}