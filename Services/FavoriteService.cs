using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Services;

public class FavoriteService : IFavoriteService
{
    private readonly ShopContext _db;

    public FavoriteService(ShopContext db) => _db = db;

    public async Task<List<FavoriteItemDto>> GetFavoritesAsync(string sessionId)
    {
        return await _db.FavoriteItems
            .Where(fi => fi.SessionId == sessionId)
            .Include(fi => fi.Chetkas)
            .ThenInclude(c => c!.Category)
            .Select(fi => new FavoriteItemDto(
                fi.Id,
                fi.ChetkasId,
                fi.Chetkas!.Name,
                fi.Chetkas!.Price,
                fi.Chetkas!.Material,
                fi.Chetkas!.Category != null ? fi.Chetkas.Category.Name : "—",
                fi.Chetkas!.Description,
                fi.Chetkas!.StockQuantity > 0
            ))
            .ToListAsync();
    }

    public async Task<int> GetFavoritesCountAsync(string sessionId)
    {
        return await _db.FavoriteItems.CountAsync(fi => fi.SessionId == sessionId);
    }

    public async Task<bool> IsFavoriteAsync(string sessionId, int chetkasId)
    {
        return await _db.FavoriteItems
            .AnyAsync(fi => fi.SessionId == sessionId && fi.ChetkasId == chetkasId);
    }

    public async Task AddFavoriteAsync(string sessionId, int chetkasId)
    {
        var exists = await IsFavoriteAsync(sessionId, chetkasId);
        if (exists) return;

        _db.FavoriteItems.Add(new FavoriteItem
        {
            SessionId = sessionId,
            ChetkasId = chetkasId
        });
        await _db.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(string sessionId, int chetkasId)
    {
        var item = await _db.FavoriteItems
            .FirstOrDefaultAsync(fi => fi.SessionId == sessionId && fi.ChetkasId == chetkasId);

        if (item is not null)
        {
            _db.FavoriteItems.Remove(item);
            await _db.SaveChangesAsync();
        }
    }

    public async Task ToggleFavoriteAsync(string sessionId, int chetkasId)
    {
        if (await IsFavoriteAsync(sessionId, chetkasId))
            await RemoveFavoriteAsync(sessionId, chetkasId);
        else
            await AddFavoriteAsync(sessionId, chetkasId);
    }
}
