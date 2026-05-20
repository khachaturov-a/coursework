using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Репозиторий для работы со списком избранных товаров.</summary>
public interface IFavoriteRepository
{
    /// <summary>Возвращает все избранные позиции сессии с включёнными товаром и категорией.</summary>
    Task<List<FavoriteItem>> GetFavoritesAsync(string sessionId);

    /// <summary>Возвращает количество товаров в избранном.</summary>
    Task<int> GetCountAsync(string sessionId);

    /// <summary>Проверяет, находится ли товар в избранном.</summary>
    Task<bool> ExistsAsync(string sessionId, int chetkasId);

    /// <summary>Ищет запись избранного по сессии и товару.</summary>
    Task<FavoriteItem?> FindAsync(string sessionId, int chetkasId);

    /// <summary>Возвращает список идентификаторов товаров в избранном.</summary>
    Task<List<int>> GetIdsAsync(string sessionId);

    /// <summary>Добавляет запись в контекст (без сохранения).</summary>
    void Add(FavoriteItem item);

    /// <summary>Помечает запись как удалённую (без сохранения).</summary>
    void Remove(FavoriteItem item);

    Task SaveChangesAsync();
}
