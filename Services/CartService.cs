using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Services;

/// <summary>Реализация <see cref="ICartService"/> на основе Entity Framework Core.</summary>
public class CartService : ICartService
{
    private readonly ShopContext _db;

    public CartService(ShopContext db) => _db = db;

    /// <inheritdoc/>
    public async Task<List<CartItemDto>> GetCartAsync(string sessionId)
    {
        return await _db.CartItems
            .Where(ci => ci.SessionId == sessionId)
            .Include(ci => ci.Chetkas)
            .ThenInclude(c => c!.Category)
            .Select(ci => new CartItemDto(
                ci.Id,
                ci.ChetkasId,
                ci.Chetkas!.Name,
                ci.Chetkas!.Price,
                ci.Quantity,
                ci.Chetkas!.Material,
                ci.Chetkas!.Category != null ? ci.Chetkas.Category.Name : "—",
                ci.Chetkas!.Price * ci.Quantity
            ))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<int> GetCartCountAsync(string sessionId)
    {
        return await _db.CartItems
            .Where(ci => ci.SessionId == sessionId)
            .SumAsync(ci => ci.Quantity);
    }

    /// <inheritdoc/>
    public async Task AddToCartAsync(string sessionId, int chetkasId, int quantity = 1)
    {
        var existing = await _db.CartItems
            .FirstOrDefaultAsync(ci => ci.SessionId == sessionId && ci.ChetkasId == chetkasId);

        if (existing is not null)
        {
            existing.Quantity = Math.Min(existing.Quantity + quantity, 100);
        }
        else
        {
            _db.CartItems.Add(new CartItem
            {
                SessionId = sessionId,
                ChetkasId = chetkasId,
                Quantity  = Math.Clamp(quantity, 1, 100)
            });
        }

        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateQuantityAsync(string sessionId, int cartItemId, int quantity)
    {
        var item = await _db.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.SessionId == sessionId);

        if (item is null) return;

        if (quantity <= 0)
        {
            _db.CartItems.Remove(item);
        }
        else
        {
            item.Quantity = Math.Clamp(quantity, 1, 100);
        }

        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveFromCartAsync(string sessionId, int cartItemId)
    {
        var item = await _db.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.SessionId == sessionId);

        if (item is not null)
        {
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task ClearCartAsync(string sessionId)
    {
        var items = _db.CartItems.Where(ci => ci.SessionId == sessionId);
        _db.CartItems.RemoveRange(items);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<decimal> GetCartTotalAsync(string sessionId)
    {
        return await _db.CartItems
            .Where(ci => ci.SessionId == sessionId)
            .Include(ci => ci.Chetkas)
            .SumAsync(ci => ci.Chetkas!.Price * ci.Quantity);
    }
}
