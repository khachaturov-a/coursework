namespace Practos3.Models;

public record ChetkasDto(
    int     Id,
    string  Name,
    decimal Price,
    int     StockQuantity,
    string  Material,
    string  CategoryName,
    string? Description,
    int     CategoryId
);

public record CategoryDto(
    int     Id,
    string  Name,
    string? Description,
    int     ProductCount
);

public record ProductsResponse(
    int                      TotalCount,
    IEnumerable<ChetkasDto>  Items
);

public record CartItemDto(
    int     Id,
    int     ChetkasId,
    string  Name,
    decimal Price,
    int     Quantity,
    string  Material,
    string  CategoryName,
    decimal Subtotal
);

public record FavoriteItemDto(
    int     Id,
    int     ChetkasId,
    string  Name,
    decimal Price,
    string  Material,
    string  CategoryName,
    string? Description,
    bool    InStock
);

public record OrderDto(
    int                    Id,
    DateTime               CreatedAt,
    decimal                TotalAmount,
    string                 Status,
    IEnumerable<OrderItemDto> Items
);

public record OrderItemDto(
    int     ChetkasId,
    string  Name,
    int     Quantity,
    decimal UnitPrice,
    decimal Subtotal
);

public record RecommendationDto(
    int     Id,
    string  Name,
    decimal Price,
    string  Material,
    string  CategoryName,
    int     CategoryId,
    string? Description,
    double  Score,
    int     StockQuantity = 0
);
