using Practos3.Models;

namespace Practos3.Services;

public interface IOrderService
{
    Task<List<OrderDto>> GetOrdersAsync(string sessionId);
    Task<OrderDto?>      GetOrderAsync(string sessionId, int orderId);
    Task<OrderDto>       PlaceOrderAsync(string sessionId);
    Task<bool>           HasPurchasedAsync(string sessionId, int chetkasId);
}
