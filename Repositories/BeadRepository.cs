using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Реализация <see cref="IBeadRepository"/> на основе EF Core.</summary>
public class BeadRepository : IBeadRepository
{
    private readonly ShopContext _db;

    public BeadRepository(ShopContext db) => _db = db;

    public async Task<IEnumerable<Chetkas>> GetAllAsync(int maxItems)
        => await _db.Chetkas.Include(c => c.Category).Take(maxItems).ToListAsync();

    public async Task<IEnumerable<Chetkas>> GetByCategoryAsync(int categoryId, int maxItems)
        => await _db.Chetkas.Include(c => c.Category)
            .Where(c => c.CategoryId == categoryId).Take(maxItems).ToListAsync();

    public async Task<Chetkas?> GetByIdAsync(int id)
        => await _db.Chetkas.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Chetkas?> FindAsync(int id)
        => await _db.Chetkas.FindAsync(id);

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
        => await _db.Categories.Include(c => c.Chetkas).ToListAsync();

    public async Task<List<Chetkas>> GetByIdsAsync(IList<int> ids)
        => await _db.Chetkas.Include(c => c.Category)
            .Where(c => ids.Contains(c.Id)).ToListAsync();

    public async Task<List<Chetkas>> GetCandidatesAsync(IReadOnlyCollection<int> excludeIds)
    {
        var excluded = excludeIds.ToHashSet();
        return await _db.Chetkas.Include(c => c.Category)
            .Where(c => !excluded.Contains(c.Id) && c.StockQuantity > 0).ToListAsync();
    }

    public async Task<List<Chetkas>> GetRandomAvailableAsync(int count)
        => await _db.Chetkas.Include(c => c.Category)
            .Where(c => c.StockQuantity > 0)
            .OrderBy(_ => EF.Functions.Random()).Take(count).ToListAsync();

    public async Task<bool> CategoryExistsAsync(int id)
        => await _db.Categories.AnyAsync(c => c.Id == id);

    public async Task<bool> CategoryNameExistsAsync(string name)
        => await _db.Categories.AnyAsync(c => c.Name == name);

    public void Add(Chetkas chetkas) => _db.Chetkas.Add(chetkas);

    public void AddCategory(Category category) => _db.Categories.Add(category);

    public async Task LoadCategoryAsync(Chetkas chetkas)
        => await _db.Entry(chetkas).Reference(c => c.Category).LoadAsync();

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
