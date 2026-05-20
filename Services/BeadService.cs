using Coursework.Models;
using Coursework.Repositories;

namespace Coursework.Services;

/// <summary>Реализация <see cref="IBeadService"/>.</summary>
public class BeadService : IBeadService
{
    private readonly IBeadRepository _repo;

    public BeadService(IBeadRepository repo) => _repo = repo;

    public async Task<IEnumerable<ChetkasDto>> GetAllAsync(int maxItems)
    {
        var items = await _repo.GetAllAsync(maxItems);
        return items.Select(MapToDto).OrderBy(c => c.Price);
    }

    public async Task<IEnumerable<ChetkasDto>> GetByCategoryAsync(int categoryId, int maxItems)
    {
        var items = await _repo.GetByCategoryAsync(categoryId, maxItems);
        return items.Select(MapToDto).OrderBy(c => c.Price);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _repo.GetCategoriesAsync();
        return categories.Select(c => new CategoryDto(c.Id, c.Name, c.Description, c.Chetkas.Count));
    }

    public async Task<ChetkasDto> CreateAsync(Chetkas chetkas)
    {
        _repo.Add(chetkas);
        await _repo.SaveChangesAsync();
        await _repo.LoadCategoryAsync(chetkas);
        return MapToDto(chetkas);
    }

    public async Task<CategoryDto> CreateCategoryAsync(Category category)
    {
        _repo.AddCategory(category);
        await _repo.SaveChangesAsync();
        return new CategoryDto(category.Id, category.Name, category.Description, 0);
    }

    private static ChetkasDto MapToDto(Chetkas c) => new(
        c.Id, c.Name, c.Price, c.StockQuantity, c.Material,
        c.Category?.Name ?? "—", c.Description, c.CategoryId);
}
