using Coursework.Models;
using Coursework.Repositories;

namespace Coursework.Services;

/// <summary>Реализация <see cref="ICartService"/>.</summary>
public class CartService : ICartService
{
    private readonly ICartRepository _repo;

    public CartService(ICartRepository repo) => _repo = repo;

    public async Task<List<CartItemDto>> GetCartAsync(string sessionId)
    {
        var items = await _repo.GetCartAsync(sessionId);
        return items.Select(ci => new CartItemDto(
            ci.Id, ci.ChetkasId, ci.Chetkas!.Name, ci.Chetkas.Price,
            ci.Quantity, ci.Chetkas.Material,
            ci.Chetkas.Category?.Name ?? "—",
            ci.Chetkas.Price * ci.Quantity)).ToList();
    }

    public async Task<int> GetCartCountAsync(string sessionId)
        => await _repo.GetTotalQuantityAsync(sessionId);

    public async Task AddToCartAsync(string sessionId, int chetkasId, int quantity = 1)
    {
        var existing = await _repo.FindItemAsync(sessionId, chetkasId);
        if (existing is not null)
            existing.Quantity = Math.Min(existing.Quantity + quantity, 100);
        else
            _repo.Add(new CartItem
            {
                SessionId = sessionId,
                ChetkasId = chetkasId,
                Quantity  = Math.Clamp(quantity, 1, 100)
            });

        await _repo.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(string sessionId, int cartItemId, int quantity)
    {
        var item = await _repo.FindByIdAsync(sessionId, cartItemId);
        if (item is null) return;

        if (quantity <= 0)
            _repo.Remove(item);
        else
            item.Quantity = Math.Clamp(quantity, 1, 100);

        await _repo.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(string sessionId, int cartItemId)
    {
        var item = await _repo.FindByIdAsync(sessionId, cartItemId);
        if (item is not null)
        {
            _repo.Remove(item);
            await _repo.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string sessionId)
    {
        _repo.Clear(sessionId);
        await _repo.SaveChangesAsync();
    }

    public async Task<decimal> GetCartTotalAsync(string sessionId)
        => await _repo.GetTotalAmountAsync(sessionId);
}
