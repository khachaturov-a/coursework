using Practos3.Models;

namespace Practos3.Services;

public interface IFavoriteService
{
    Task<List<FavoriteItemDto>> GetFavoritesAsync(string sessionId);
    Task<int>                   GetFavoritesCountAsync(string sessionId);
    Task<bool>                  IsFavoriteAsync(string sessionId, int chetkasId);
    Task                        AddFavoriteAsync(string sessionId, int chetkasId);
    Task                        RemoveFavoriteAsync(string sessionId, int chetkasId);
    Task                        ToggleFavoriteAsync(string sessionId, int chetkasId);
}
