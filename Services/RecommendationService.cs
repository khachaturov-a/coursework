using Coursework.Models;
using Coursework.Repositories;

namespace Coursework.Services;

/// <summary>
/// Контентная фильтрация на основе истории взаимодействий пользователя.
/// Покупки (вес 5), избранное (вес 3), просмотры с затуханием e^(-0.1·дней) (вес 1).
/// </summary>
public class RecommendationService : IRecommendationService
{
    private readonly IBeadRepository        _beadRepo;
    private readonly IFavoriteRepository    _favRepo;
    private readonly IOrderRepository       _orderRepo;
    private readonly IProductViewRepository _viewRepo;

    public RecommendationService(
        IBeadRepository beadRepo, IFavoriteRepository favRepo,
        IOrderRepository orderRepo, IProductViewRepository viewRepo)
    {
        _beadRepo  = beadRepo;
        _favRepo   = favRepo;
        _orderRepo = orderRepo;
        _viewRepo  = viewRepo;
    }

    public async Task RecordViewAsync(string sessionId, int chetkasId)
    {
        var recent = await _viewRepo.GetRecentViewAsync(sessionId, chetkasId);
        // Не дублируем просмотр одного товара в течение 30 минут
        if (recent is not null && (DateTime.UtcNow - recent.ViewedAt).TotalMinutes < 30)
            return;

        _viewRepo.Add(new ProductView { SessionId = sessionId, ChetkasId = chetkasId });
        await _viewRepo.SaveChangesAsync();
    }

    public async Task<List<RecommendationDto>> GetRecommendationsAsync(string sessionId, int count = 8)
    {
        var weights = new Dictionary<int, double>();

        foreach (var id in await _orderRepo.GetPurchasedIdsAsync(sessionId))
            Accumulate(weights, id, 5.0);

        foreach (var id in await _favRepo.GetIdsAsync(sessionId))
            Accumulate(weights, id, 3.0);

        foreach (var v in await _viewRepo.GetViewsAsync(sessionId))
            Accumulate(weights, v.ChetkasId, Math.Exp(-0.1 * (DateTime.UtcNow - v.ViewedAt).TotalDays));

        if (!weights.Any())
            return await GetPopularAsync(count);

        var interactedIds = weights.Keys.ToList();
        var interacted    = await _beadRepo.GetByIdsAsync(interactedIds);

        var catPrefs = new Dictionary<int, double>();
        var matPrefs = new Dictionary<string, double>();
        var priceSum = 0.0;
        var totalW   = 0.0;

        foreach (var p in interacted)
        {
            var w = weights[p.Id];
            Accumulate(catPrefs, p.CategoryId, w);
            Accumulate(matPrefs, p.Material.ToLowerInvariant(), w);
            priceSum += (double)p.Price * w;
            totalW   += w;
        }

        var avgPrice   = totalW > 0 ? priceSum / totalW : 1000.0;
        var candidates = await _beadRepo.GetCandidatesAsync(interactedIds.ToHashSet());

        return candidates
            .Select(c =>
            {
                catPrefs.TryGetValue(c.CategoryId, out var catScore);
                matPrefs.TryGetValue(c.Material.ToLowerInvariant(), out var matScore);
                var priceDiff  = Math.Abs((double)c.Price - avgPrice) / Math.Max(avgPrice, 1);
                var score      = catScore * 2.0 + matScore * 1.5 + Math.Max(0.0, 1.0 - priceDiff) * 0.5;
                return (Product: c, Score: score);
            })
            .OrderByDescending(x => x.Score)
            .Take(count)
            .Select(x => MapToDto(x.Product, x.Score))
            .ToList();
    }

    public async Task<List<RecommendationDto>> GetSimilarAsync(int chetkasId, int count = 6)
    {
        var source = await _beadRepo.GetByIdAsync(chetkasId);
        if (source is null) return new List<RecommendationDto>();

        var avgPrice   = (double)source.Price;
        var candidates = await _beadRepo.GetCandidatesAsync(new HashSet<int> { chetkasId });

        return candidates
            .Select(c =>
            {
                var score = 0.0;
                if (c.CategoryId == source.CategoryId) score += 3.0;
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

    private async Task<List<RecommendationDto>> GetPopularAsync(int count)
    {
        var topIds = await _orderRepo.GetTopProductIdsAsync(count);

        var products = topIds.Count >= count
            ? (await _beadRepo.GetByIdsAsync(topIds)).Where(c => c.StockQuantity > 0).ToList()
            : await _beadRepo.GetRandomAvailableAsync(count);

        return products.Select(p => MapToDto(p, 0)).ToList();
    }

    private static void Accumulate<TKey>(Dictionary<TKey, double> dict, TKey key, double value) where TKey : notnull
    {
        dict.TryGetValue(key, out var existing);
        dict[key] = existing + value;
    }

    private static RecommendationDto MapToDto(Chetkas c, double score) => new(
        c.Id, c.Name, c.Price, c.Material,
        c.Category?.Name ?? "—", c.CategoryId, c.Description,
        Math.Round(score, 3), c.StockQuantity);
}
