using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Репозиторий для работы с корзиной покупателя.</summary>
public interface ICartRepository
{
    /// <summary>Возвращает все позиции корзины сессии с включёнными товаром и категорией.</summary>
    Task<List<CartItem>> GetCartAsync(string sessionId);

    /// <summary>Ищет позицию корзины по сессии и товару.</summary>
    Task<CartItem?> FindItemAsync(string sessionId, int chetkasId);

    /// <summary>Ищет позицию корзины по идентификатору записи.</summary>
    Task<CartItem?> FindByIdAsync(string sessionId, int cartItemId);

    /// <summary>Возвращает суммарное количество единиц в корзине.</summary>
    Task<int> GetTotalQuantityAsync(string sessionId);

    /// <summary>Возвращает итоговую сумму корзины.</summary>
    Task<decimal> GetTotalAmountAsync(string sessionId);

    /// <summary>Добавляет позицию в контекст (без сохранения).</summary>
    void Add(CartItem item);

    /// <summary>Помечает позицию как удалённую (без сохранения).</summary>
    void Remove(CartItem item);

    /// <summary>Помечает все позиции сессии как удалённые (без сохранения).</summary>
    void Clear(string sessionId);

    Task SaveChangesAsync();
}
