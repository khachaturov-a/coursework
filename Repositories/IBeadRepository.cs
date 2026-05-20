using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Репозиторий для работы с каталогом чёток и категориями.</summary>
public interface IBeadRepository
{
    /// <summary>Возвращает список чёток (не более <paramref name="maxItems"/>), включая категорию.</summary>
    Task<IEnumerable<Chetkas>> GetAllAsync(int maxItems);

    /// <summary>Возвращает чётки указанной категории (не более <paramref name="maxItems"/>).</summary>
    Task<IEnumerable<Chetkas>> GetByCategoryAsync(int categoryId, int maxItems);

    /// <summary>Возвращает товар по идентификатору с включённой категорией, или <c>null</c>.</summary>
    Task<Chetkas?> GetByIdAsync(int id);

    /// <summary>Возвращает отслеживаемый экземпляр товара для обновления, или <c>null</c>.</summary>
    Task<Chetkas?> FindAsync(int id);

    /// <summary>Возвращает все категории с коллекцией товаров (для подсчёта).</summary>
    Task<IEnumerable<Category>> GetCategoriesAsync();

    /// <summary>Возвращает товары с указанными идентификаторами, включая категорию.</summary>
    Task<List<Chetkas>> GetByIdsAsync(IList<int> ids);

    /// <summary>Возвращает товары в наличии, не входящие в <paramref name="excludeIds"/>, с категорией.</summary>
    Task<List<Chetkas>> GetCandidatesAsync(IReadOnlyCollection<int> excludeIds);

    /// <summary>Возвращает случайную выборку товаров в наличии.</summary>
    Task<List<Chetkas>> GetRandomAvailableAsync(int count);

    /// <summary>Проверяет существование категории по идентификатору.</summary>
    Task<bool> CategoryExistsAsync(int id);

    /// <summary>Проверяет, существует ли категория с указанным именем.</summary>
    Task<bool> CategoryNameExistsAsync(string name);

    /// <summary>Добавляет товар в контекст (без сохранения).</summary>
    void Add(Chetkas chetkas);

    /// <summary>Добавляет категорию в контекст (без сохранения).</summary>
    void AddCategory(Category category);

    /// <summary>Подгружает навигационное свойство <see cref="Chetkas.Category"/>.</summary>
    Task LoadCategoryAsync(Chetkas chetkas);

    Task SaveChangesAsync();
}
