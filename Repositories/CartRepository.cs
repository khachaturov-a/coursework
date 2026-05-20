using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Реализация <see cref="ICartRepository"/> на основе EF Core.</summary>
public class CartRepository : ICartRepository
{
    private readonly ShopContext _db;

    public CartRepository(ShopContext db) => _db = db;

    public async Task<List<CartItem>> GetCartAsync(string sessionId)
        => await _db.CartItems
            .Where(ci => ci.SessionId == sessionId)
            .Include(ci => ci.Chetkas).ThenInclude(c => c!.Category)
            .ToListAsync();

    public async Task<CartItem?> FindItemAsync(string sessionId, int chetkasId)
        => await _db.CartItems.FirstOrDefaultAsync(
            ci => ci.SessionId == sessionId && ci.ChetkasId == chetkasId);

    public async Task<CartItem?> FindByIdAsync(string sessionId, int cartItemId)
        => await _db.CartItems.FirstOrDefaultAsync(
            ci => ci.Id == cartItemId && ci.SessionId == sessionId);

    public async Task<int> GetTotalQuantityAsync(string sessionId)
        => await _db.CartItems.Where(ci => ci.SessionId == sessionId).SumAsync(ci => ci.Quantity);

    public async Task<decimal> GetTotalAmountAsync(string sessionId)
        => await _db.CartItems
            .Where(ci => ci.SessionId == sessionId)
            .Include(ci => ci.Chetkas)
            .SumAsync(ci => ci.Chetkas!.Price * ci.Quantity);

    public void Add(CartItem item) => _db.CartItems.Add(item);

    public void Remove(CartItem item) => _db.CartItems.Remove(item);

    public void Clear(string sessionId)
        => _db.CartItems.RemoveRange(_db.CartItems.Where(ci => ci.SessionId == sessionId));

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
