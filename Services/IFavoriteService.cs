using Coursework.Models;

namespace Coursework.Services;

/// <summary>Сервис управления списком избранных товаров в рамках сессии.</summary>
public interface IFavoriteService
{
    /// <summary>Возвращает все избранные товары для указанной сессии.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    Task<List<FavoriteItemDto>> GetFavoritesAsync(string sessionId);

    /// <summary>Возвращает количество товаров в избранном для указанной сессии.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    Task<int> GetFavoritesCountAsync(string sessionId);

    /// <summary>Проверяет, находится ли товар в избранном у данной сессии.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="chetkasId">Идентификатор товара.</param>
    Task<bool> IsFavoriteAsync(string sessionId, int chetkasId);

    /// <summary>Добавляет товар в избранное. Повторный вызов игнорируется.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="chetkasId">Идентификатор товара.</param>
    Task AddFavoriteAsync(string sessionId, int chetkasId);

    /// <summary>Удаляет товар из избранного.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="chetkasId">Идентификатор товара.</param>
    Task RemoveFavoriteAsync(string sessionId, int chetkasId);

    /// <summary>Переключает состояние избранного: добавляет, если отсутствует, иначе удаляет.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="chetkasId">Идентификатор товара.</param>
    Task ToggleFavoriteAsync(string sessionId, int chetkasId);
}
