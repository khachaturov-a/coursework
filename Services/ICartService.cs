using Coursework.Models;

namespace Coursework.Services;

/// <summary>Сервис управления корзиной покупателя в рамках текущей сессии.</summary>
public interface ICartService
{
    /// <summary>Возвращает все позиции корзины для указанной сессии.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    Task<List<CartItemDto>> GetCartAsync(string sessionId);

    /// <summary>Возвращает суммарное количество единиц товара в корзине.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    Task<int> GetCartCountAsync(string sessionId);

    /// <summary>Добавляет товар в корзину. Если товар уже есть, увеличивает количество.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="chetkasId">Идентификатор добавляемого товара.</param>
    /// <param name="quantity">Количество единиц (по умолчанию 1).</param>
    Task AddToCartAsync(string sessionId, int chetkasId, int quantity = 1);

    /// <summary>Изменяет количество единиц указанной позиции корзины. При quantity ≤ 0 удаляет позицию.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="cartItemId">Идентификатор записи корзины.</param>
    /// <param name="quantity">Новое количество единиц.</param>
    Task UpdateQuantityAsync(string sessionId, int cartItemId, int quantity);

    /// <summary>Удаляет указанную позицию из корзины.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="cartItemId">Идентификатор записи корзины.</param>
    Task RemoveFromCartAsync(string sessionId, int cartItemId);

    /// <summary>Полностью очищает корзину сессии.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    Task ClearCartAsync(string sessionId);

    /// <summary>Возвращает итоговую сумму заказа для корзины сессии.</summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    Task<decimal> GetCartTotalAsync(string sessionId);
}
