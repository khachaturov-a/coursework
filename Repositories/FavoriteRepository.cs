using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Реализация <see cref="IFavoriteRepository"/> на основе EF Core.</summary>
public class FavoriteRepository : IFavoriteRepository
{
    private readonly ShopContext _db;

    public FavoriteRepository(ShopContext db) => _db = db;

    public async Task<List<FavoriteItem>> GetFavoritesAsync(string sessionId)
        => await _db.FavoriteItems
            .Where(fi => fi.SessionId == sessionId)
            .Include(fi => fi.Chetkas).ThenInclude(c => c!.Category)
            .ToListAsync();

    public async Task<int> GetCountAsync(string sessionId)
        => await _db.FavoriteItems.CountAsync(fi => fi.SessionId == sessionId);

    public async Task<bool> ExistsAsync(string sessionId, int chetkasId)
        => await _db.FavoriteItems.AnyAsync(
            fi => fi.SessionId == sessionId && fi.ChetkasId == chetkasId);

    public async Task<FavoriteItem?> FindAsync(string sessionId, int chetkasId)
        => await _db.FavoriteItems.FirstOrDefaultAsync(
            fi => fi.SessionId == sessionId && fi.ChetkasId == chetkasId);

    public async Task<List<int>> GetIdsAsync(string sessionId)
        => await _db.FavoriteItems
            .Where(fi => fi.SessionId == sessionId)
            .Select(fi => fi.ChetkasId).ToListAsync();

    public void Add(FavoriteItem item) => _db.FavoriteItems.Add(item);

    public void Remove(FavoriteItem item) => _db.FavoriteItems.Remove(item);

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
