using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Репозиторий для работы с заказами и позициями заказов.</summary>
public interface IOrderRepository
{
    /// <summary>Возвращает историю заказов сессии с позициями и товарами, отсортированную по убыванию даты.</summary>
    Task<List<Order>> GetOrdersAsync(string sessionId);

    /// <summary>Возвращает конкретный заказ сессии с позициями и товарами, или <c>null</c>.</summary>
    Task<Order?> GetByIdAsync(string sessionId, int orderId);

    /// <summary>Возвращает идентификаторы товаров, купленных в рамках сессии.</summary>
    Task<List<int>> GetPurchasedIdsAsync(string sessionId);

    /// <summary>Проверяет, покупал ли пользователь сессии указанный товар.</summary>
    Task<bool> HasPurchasedAsync(string sessionId, int chetkasId);

    /// <summary>Возвращает идентификаторы топовых товаров по количеству продаж.</summary>
    Task<List<int>> GetTopProductIdsAsync(int count);

    /// <summary>Добавляет заказ в контекст (без сохранения).</summary>
    void Add(Order order);

    Task SaveChangesAsync();
}
