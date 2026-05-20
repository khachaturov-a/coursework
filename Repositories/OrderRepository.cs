using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Repositories;

/// <summary>Реализация <see cref="IOrderRepository"/> на основе EF Core.</summary>
public class OrderRepository : IOrderRepository
{
    private readonly ShopContext _db;

    public OrderRepository(ShopContext db) => _db = db;

    public async Task<List<Order>> GetOrdersAsync(string sessionId)
        => await _db.Orders
            .Where(o => o.SessionId == sessionId)
            .Include(o => o.Items).ThenInclude(oi => oi.Chetkas)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

    public async Task<Order?> GetByIdAsync(string sessionId, int orderId)
        => await _db.Orders
            .Where(o => o.Id == orderId && o.SessionId == sessionId)
            .Include(o => o.Items).ThenInclude(oi => oi.Chetkas)
            .FirstOrDefaultAsync();

    public async Task<List<int>> GetPurchasedIdsAsync(string sessionId)
        => await _db.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.Order!.SessionId == sessionId)
            .Select(oi => oi.ChetkasId).Distinct().ToListAsync();

    public async Task<bool> HasPurchasedAsync(string sessionId, int chetkasId)
        => await _db.OrderItems
            .Include(oi => oi.Order)
            .AnyAsync(oi => oi.ChetkasId == chetkasId && oi.Order!.SessionId == sessionId);

    public async Task<List<int>> GetTopProductIdsAsync(int count)
        => await _db.OrderItems
            .GroupBy(oi => oi.ChetkasId)
            .OrderByDescending(g => g.Sum(oi => oi.Quantity))
            .Select(g => g.Key).Take(count).ToListAsync();

    public void Add(Order order) => _db.Orders.Add(order);

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
