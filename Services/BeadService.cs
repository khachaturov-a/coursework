using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Services;

/// <summary>Реализация <see cref="IBeadService"/> на основе Entity Framework Core.</summary>
public class BeadService : IBeadService
{
    private readonly ShopContext _db;

    public BeadService(ShopContext db) => _db = db;

    /// <inheritdoc/>
    public async Task<IEnumerable<ChetkasDto>> GetAllAsync(int maxItems)
    {
        var items = await _db.Chetkas
            .Include(c => c.Category)
            .Take(maxItems)
            .Select(c => new ChetkasDto(
                c.Id, c.Name, c.Price, c.StockQuantity, c.Material,
                c.Category != null ? c.Category.Name : "—",
                c.Description,
                c.CategoryId))
            .ToListAsync();

        return items.OrderBy(c => c.Price);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        return await _db.Categories
            .Select(cat => new CategoryDto(
                cat.Id,
                cat.Name,
                cat.Description,
                cat.Chetkas.Count))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ChetkasDto>> GetByCategoryAsync(int categoryId, int maxItems)
    {
        var items = await _db.Chetkas
            .Include(c => c.Category)
            .Where(c => c.CategoryId == categoryId)
            .Take(maxItems)
            .Select(c => new ChetkasDto(
                c.Id, c.Name, c.Price, c.StockQuantity, c.Material,
                c.Category != null ? c.Category.Name : "—",
                c.Description,
                c.CategoryId))
            .ToListAsync();

        return items.OrderBy(c => c.Price);
    }

    /// <inheritdoc/>
    public async Task<ChetkasDto> CreateAsync(Chetkas chetkas)
    {
        _db.Chetkas.Add(chetkas);
        await _db.SaveChangesAsync();
        await _db.Entry(chetkas).Reference(c => c.Category).LoadAsync();

        return new ChetkasDto(
            chetkas.Id, chetkas.Name, chetkas.Price,
            chetkas.StockQuantity, chetkas.Material,
            chetkas.Category?.Name ?? "—",
            chetkas.Description,
            chetkas.CategoryId);
    }

    /// <inheritdoc/>
    public async Task<CategoryDto> CreateCategoryAsync(Category category)
    {
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return new CategoryDto(category.Id, category.Name, category.Description, 0);
    }
}
