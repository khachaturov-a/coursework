using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Services;

/// <summary>
/// Контентная фильтрация на основе истории взаимодействий пользователя.
/// Покупки (вес 5), избранное (вес 3), просмотры с временны́м затуханием (вес 1×e^(-0.1d)).
/// </summary>
public class RecommendationService : IRecommendationService
{
    private readonly ShopContext _db;

    public RecommendationService(ShopContext db) => _db = db;

    /// <inheritdoc/>
    public async Task RecordViewAsync(string sessionId, int chetkasId)
    {
        // Не дублируем просмотр одного товара за последние 30 минут
        var recentView = await _db.ProductViews
            .Where(pv => pv.SessionId == sessionId && pv.ChetkasId == chetkasId)
            .OrderByDescending(pv => pv.ViewedAt)
            .FirstOrDefaultAsync();

        if (recentView is not null && (DateTime.UtcNow - recentView.ViewedAt).TotalMinutes < 30)
            return;

        _db.ProductViews.Add(new ProductView
        {
            SessionId = sessionId,
            ChetkasId = chetkasId
        });
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<List<RecommendationDto>> GetRecommendationsAsync(string sessionId, int count = 8)
    {
        // --- 1. Собираем веса взаимодействий ---
        var weights = new Dictionary<int, double>();

        // Покупки (вес 5)
        var purchasedIds = await _db.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.Order!.SessionId == sessionId)
            .Select(oi => oi.ChetkasId)
            .Distinct()
            .ToListAsync();
        foreach (var id in purchasedIds) Accumulate(weights, id, 5.0);

        // Избранное (вес 3)
        var favoriteIds = await _db.FavoriteItems
            .Where(fi => fi.SessionId == sessionId)
            .Select(fi => fi.ChetkasId)
            .ToListAsync();
        foreach (var id in favoriteIds) Accumulate(weights, id, 3.0);

        // Просмотры с временны́м затуханием
        var views = await _db.ProductViews
            .Where(pv => pv.SessionId == sessionId)
            .Select(pv => new { pv.ChetkasId, pv.ViewedAt })
            .ToListAsync();
        foreach (var v in views)
        {
            var days = (DateTime.UtcNow - v.ViewedAt).TotalDays;
            var decayed = Math.Exp(-0.1 * days);
            Accumulate(weights, v.ChetkasId, decayed);
        }

        // Нет истории — возвращаем популярные товары
        if (!weights.Any())
            return await GetPopularAsync(count);

        // --- 2. Строим профиль предпочтений ---
        var interactedIds = weights.Keys.ToList();
        var interacted = await _db.Chetkas
            .Include(c => c.Category)
            .Where(c => interactedIds.Contains(c.Id))
            .ToListAsync();

        var catPrefs  = new Dictionary<int, double>();
        var matPrefs  = new Dictionary<string, double>();
        var priceSum  = 0.0;
        var totalW    = 0.0;

        foreach (var p in interacted)
        {
            var w = weights[p.Id];
            Accumulate(catPrefs, p.CategoryId, w);
            Accumulate(matPrefs, p.Material.ToLowerInvariant(), w);
            priceSum += (double)p.Price * w;
            totalW   += w;
        }

        var avgPrice = totalW > 0 ? priceSum / totalW : 1000.0;

        // --- 3. Скорим кандидатов ---
        var excludeIds = interactedIds.ToHashSet();
        var candidates = await _db.Chetkas
            .Include(c => c.Category)
            .Where(c => !excludeIds.Contains(c.Id) && c.StockQuantity > 0)
            .ToListAsync();

        var result = candidates
            .Select(c =>
            {
                catPrefs.TryGetValue(c.CategoryId, out var catScore);
                matPrefs.TryGetValue(c.Material.ToLowerInvariant(), out var matScore);

                var priceDiff   = Math.Abs((double)c.Price - avgPrice) / Math.Max(avgPrice, 1);
                var priceScore  = Math.Max(0.0, 1.0 - priceDiff) * 0.5;
                var finalScore  = catScore * 2.0 + matScore * 1.5 + priceScore;

                return (Product: c, Score: finalScore);
            })
            .OrderByDescending(x => x.Score)
            .Take(count)
            .Select(x => MapToDto(x.Product, x.Score))
            .ToList();

        return result;
    }

    /// <inheritdoc/>
    public async Task<List<RecommendationDto>> GetSimilarAsync(int chetkasId, int count = 6)
    {
        var source = await _db.Chetkas
            .Include(c => c.Category)
            .FirstOrDefaultAsync(c => c.Id == chetkasId);

        if (source is null) return new List<RecommendationDto>();

        var candidates = await _db.Chetkas
            .Include(c => c.Category)
            .Where(c => c.Id != chetkasId && c.StockQuantity > 0)
            .ToListAsync();

        var avgPrice = (double)source.Price;

        return candidates
            .Select(c =>
            {
                var score = 0.0;
                if (c.CategoryId == source.CategoryId)              score += 3.0;
                if (c.Material.Equals(source.Material, StringComparison.OrdinalIgnoreCase)) score += 2.0;

                var priceDiff = Math.Abs((double)c.Price - avgPrice) / Math.Max(avgPrice, 1);
                score += Math.Max(0.0, 1.0 - priceDiff) * 0.5;

                return (Product: c, Score: score);
            })
            .OrderByDescending(x => x.Score)
            .Take(count)
            .Select(x => MapToDto(x.Product, x.Score))
            .ToList();
    }

    // ---- helpers ----

    private async Task<List<RecommendationDto>> GetPopularAsync(int count)
    {
        // «Популярные» = наибольшее количество продаж
        var topIds = await _db.OrderItems
            .GroupBy(oi => oi.ChetkasId)
            .OrderByDescending(g => g.Sum(oi => oi.Quantity))
            .Select(g => g.Key)
            .Take(count)
            .ToListAsync();

        List<Chetkas> products;

        if (topIds.Count >= count)
        {
            products = await _db.Chetkas
                .Include(c => c.Category)
                .Where(c => topIds.Contains(c.Id) && c.StockQuantity > 0)
                .ToListAsync();
        }
        else
        {
            // Нет продаж — случайная выборка
            products = await _db.Chetkas
                .Include(c => c.Category)
                .Where(c => c.StockQuantity > 0)
                .OrderBy(_ => EF.Functions.Random())
                .Take(count)
                .ToListAsync();
        }

        return products.Select(p => MapToDto(p, 0)).ToList();
    }

    private static void Accumulate<TKey>(Dictionary<TKey, double> dict, TKey key, double value) where TKey : notnull
    {
        dict.TryGetValue(key, out var existing);
        dict[key] = existing + value;
    }

    private static RecommendationDto MapToDto(Chetkas c, double score) => new(
        c.Id,
        c.Name,
        c.Price,
        c.Material,
        c.Category?.Name ?? "—",
        c.CategoryId,
        c.Description,
        Math.Round(score, 3),
        c.StockQuantity
    );
}
