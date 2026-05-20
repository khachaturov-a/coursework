using Coursework.Models;
using Coursework.Repositories;

namespace Coursework.Services;

/// <summary>Реализация <see cref="IFavoriteService"/>.</summary>
public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _repo;

    public FavoriteService(IFavoriteRepository repo) => _repo = repo;

    public async Task<List<FavoriteItemDto>> GetFavoritesAsync(string sessionId)
    {
        var items = await _repo.GetFavoritesAsync(sessionId);
        return items.Select(fi => new FavoriteItemDto(
            fi.Id, fi.ChetkasId, fi.Chetkas!.Name, fi.Chetkas.Price,
            fi.Chetkas.Material,
            fi.Chetkas.Category?.Name ?? "—",
            fi.Chetkas.Description,
            fi.Chetkas.StockQuantity > 0)).ToList();
    }

    public async Task<int> GetFavoritesCountAsync(string sessionId)
        => await _repo.GetCountAsync(sessionId);

    public async Task<bool> IsFavoriteAsync(string sessionId, int chetkasId)
        => await _repo.ExistsAsync(sessionId, chetkasId);

    public async Task AddFavoriteAsync(string sessionId, int chetkasId)
    {
        if (await _repo.ExistsAsync(sessionId, chetkasId)) return;

        _repo.Add(new FavoriteItem { SessionId = sessionId, ChetkasId = chetkasId });
        await _repo.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(string sessionId, int chetkasId)
    {
        var item = await _repo.FindAsync(sessionId, chetkasId);
        if (item is not null)
        {
            _repo.Remove(item);
            await _repo.SaveChangesAsync();
        }
    }

    public async Task ToggleFavoriteAsync(string sessionId, int chetkasId)
    {
        if (await _repo.ExistsAsync(sessionId, chetkasId))
            await RemoveFavoriteAsync(sessionId, chetkasId);
        else
            await AddFavoriteAsync(sessionId, chetkasId);
    }
}
