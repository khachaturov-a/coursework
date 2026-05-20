using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Репозиторий для работы с историей просмотров товаров.</summary>
public interface IProductViewRepository
{
    /// <summary>Возвращает последний просмотр указанного товара в сессии, или <c>null</c>.</summary>
    Task<ProductView?> GetRecentViewAsync(string sessionId, int chetkasId);

    /// <summary>Возвращает все просмотры для указанной сессии.</summary>
    Task<List<ProductView>> GetViewsAsync(string sessionId);

    /// <summary>Добавляет запись просмотра в контекст (без сохранения).</summary>
    void Add(ProductView view);

    Task SaveChangesAsync();
}
