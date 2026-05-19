using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Services;

/// <summary>Реализация <see cref="IOrderService"/> на основе Entity Framework Core.</summary>
public class OrderService : IOrderService
{
    private readonly ShopContext  _db;
    private readonly ICartService _cart;

    public OrderService(ShopContext db, ICartService cart)
    {
        _db   = db;
        _cart = cart;
    }

    /// <inheritdoc/>
    public async Task<List<OrderDto>> GetOrdersAsync(string sessionId)
    {
        var orders = await _db.Orders
            .Where(o => o.SessionId == sessionId)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Chetkas)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(MapToDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<OrderDto?> GetOrderAsync(string sessionId, int orderId)
    {
        var order = await _db.Orders
            .Where(o => o.Id == orderId && o.SessionId == sessionId)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Chetkas)
            .FirstOrDefaultAsync();

        return order is null ? null : MapToDto(order);
    }

    /// <inheritdoc/>
    public async Task<OrderDto> PlaceOrderAsync(string sessionId)
    {
        var cartItems = await _db.CartItems
            .Where(ci => ci.SessionId == sessionId)
            .Include(ci => ci.Chetkas)
            .ToListAsync();

        if (!cartItems.Any())
            throw new InvalidOperationException("Корзина пуста");

        var order = new Order
        {
            SessionId   = sessionId,
            TotalAmount = cartItems.Sum(ci => ci.Chetkas!.Price * ci.Quantity),
            Status      = "Обрабатывается"
        };

        foreach (var ci in cartItems)
        {
            order.Items.Add(new OrderItem
            {
                ChetkasId = ci.ChetkasId,
                Quantity  = ci.Quantity,
                UnitPrice = ci.Chetkas!.Price
            });

            // Уменьшаем остаток на складе
            var product = await _db.Chetkas.FindAsync(ci.ChetkasId);
            if (product is not null)
                product.StockQuantity = Math.Max(0, product.StockQuantity - ci.Quantity);
        }

        _db.Orders.Add(order);
        _db.CartItems.RemoveRange(cartItems);
        await _db.SaveChangesAsync();

        return MapToDto(order);
    }

    /// <inheritdoc/>
    public async Task<bool> HasPurchasedAsync(string sessionId, int chetkasId)
    {
        return await _db.OrderItems
            .Include(oi => oi.Order)
            .AnyAsync(oi => oi.ChetkasId == chetkasId && oi.Order!.SessionId == sessionId);
    }

    private static OrderDto MapToDto(Order order) => new(
        order.Id,
        order.CreatedAt,
        order.TotalAmount,
        order.Status,
        order.Items.Select(oi => new OrderItemDto(
            oi.ChetkasId,
            oi.Chetkas?.Name ?? "—",
            oi.Quantity,
            oi.UnitPrice,
            oi.UnitPrice * oi.Quantity
        ))
    );
}
