using Practos3.Models;

namespace Practos3.Services;

public interface ICartService
{
    Task<List<CartItemDto>> GetCartAsync(string sessionId);
    Task<int>               GetCartCountAsync(string sessionId);
    Task                    AddToCartAsync(string sessionId, int chetkasId, int quantity = 1);
    Task                    UpdateQuantityAsync(string sessionId, int cartItemId, int quantity);
    Task                    RemoveFromCartAsync(string sessionId, int cartItemId);
    Task                    ClearCartAsync(string sessionId);
    Task<decimal>           GetCartTotalAsync(string sessionId);
}
