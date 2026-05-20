using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Реализация <see cref="IProductViewRepository"/> на основе EF Core.</summary>
public class ProductViewRepository : IProductViewRepository
{
    private readonly ShopContext _db;

    public ProductViewRepository(ShopContext db) => _db = db;

    public async Task<ProductView?> GetRecentViewAsync(string sessionId, int chetkasId)
        => await _db.ProductViews
            .Where(pv => pv.SessionId == sessionId && pv.ChetkasId == chetkasId)
            .OrderByDescending(pv => pv.ViewedAt)
            .FirstOrDefaultAsync();

    public async Task<List<ProductView>> GetViewsAsync(string sessionId)
        => await _db.ProductViews.Where(pv => pv.SessionId == sessionId).ToListAsync();

    public void Add(ProductView view) => _db.ProductViews.Add(view);

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
