using Coursework.Models;
using Coursework.Repositories;

namespace Coursework.Services;

/// <summary>Реализация <see cref="IOrderService"/>.</summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly ICartRepository  _cartRepo;
    private readonly IBeadRepository  _beadRepo;

    public OrderService(IOrderRepository orderRepo, ICartRepository cartRepo, IBeadRepository beadRepo)
    {
        _orderRepo = orderRepo;
        _cartRepo  = cartRepo;
        _beadRepo  = beadRepo;
    }

    public async Task<List<OrderDto>> GetOrdersAsync(string sessionId)
        => (await _orderRepo.GetOrdersAsync(sessionId)).Select(MapToDto).ToList();

    public async Task<OrderDto?> GetOrderAsync(string sessionId, int orderId)
    {
        var order = await _orderRepo.GetByIdAsync(sessionId, orderId);
        return order is null ? null : MapToDto(order);
    }

    public async Task<OrderDto> PlaceOrderAsync(string sessionId)
    {
        var cartItems = await _cartRepo.GetCartAsync(sessionId);
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

            var product = await _beadRepo.FindAsync(ci.ChetkasId);
            if (product is not null)
                product.StockQuantity = Math.Max(0, product.StockQuantity - ci.Quantity);
        }

        _orderRepo.Add(order);
        _cartRepo.Clear(sessionId);
        await _orderRepo.SaveChangesAsync();

        return MapToDto(order);
    }

    public async Task<bool> HasPurchasedAsync(string sessionId, int chetkasId)
        => await _orderRepo.HasPurchasedAsync(sessionId, chetkasId);

    private static OrderDto MapToDto(Order order) => new(
        order.Id, order.CreatedAt, order.TotalAmount, order.Status,
        order.Items.Select(oi => new OrderItemDto(
            oi.ChetkasId, oi.Chetkas?.Name ?? "—",
            oi.Quantity, oi.UnitPrice, oi.UnitPrice * oi.Quantity)));
}
