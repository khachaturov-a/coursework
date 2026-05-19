using Coursework.Models;

namespace Coursework.Services;

/// <summary>Сервис для работы с каталогом чёток и категорий.</summary>
public interface IBeadService
{
    /// <summary>Возвращает список всех чёток, отсортированных по цене.</summary>
    /// <param name="maxItems">Максимальное количество возвращаемых записей.</param>
    Task<IEnumerable<ChetkasDto>> GetAllAsync(int maxItems);

    /// <summary>Возвращает чётки, принадлежащие указанной категории.</summary>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="maxItems">Максимальное количество возвращаемых записей.</param>
    Task<IEnumerable<ChetkasDto>> GetByCategoryAsync(int categoryId, int maxItems);

    /// <summary>Возвращает список всех категорий с количеством товаров в каждой.</summary>
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();

    /// <summary>Создаёт новую запись чёток и сохраняет её в базе данных.</summary>
    /// <param name="chetkas">Данные нового товара.</param>
    /// <returns>DTO созданного товара.</returns>
    Task<ChetkasDto> CreateAsync(Chetkas chetkas);

    /// <summary>Создаёт новую категорию и сохраняет её в базе данных.</summary>
    /// <param name="category">Данные новой категории.</param>
    /// <returns>DTO созданной категории.</returns>
    Task<CategoryDto> CreateCategoryAsync(Category category);
}

