using Coursework.Models;

namespace Coursework.Services;

/// <summary>Сервис управления заказами покупателя.</summary>
public interface IOrderService
{
    /// <summary>Возвращает историю заказов для указанной сессии, отсортированную по дате убывания.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    Task<List<OrderDto>> GetOrdersAsync(string sessionId);

    /// <summary>Возвращает конкретный заказ сессии или <c>null</c>, если заказ не найден.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="orderId">Идентификатор заказа.</param>
    Task<OrderDto?> GetOrderAsync(string sessionId, int orderId);

    /// <summary>
    /// Оформляет заказ из текущей корзины сессии: создаёт запись Order, списывает остатки
    /// со склада и очищает корзину.
    /// </summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <exception cref="InvalidOperationException">Корзина пуста.</exception>
    Task<OrderDto> PlaceOrderAsync(string sessionId);

    /// <summary>Проверяет, покупал ли пользователь данной сессии указанный товар ранее.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="chetkasId">Идентификатор товара.</param>
    Task<bool> HasPurchasedAsync(string sessionId, int chetkasId);
}
