using Coursework.Models;

namespace Coursework.Services;

/// <summary>Сервис интеллектуальных рекомендаций на основе истории взаимодействий пользователя.</summary>
public interface IRecommendationService
{
    /// <summary>
    /// Возвращает персонализированные рекомендации для сессии на основе покупок,
    /// избранного и просмотров с экспоненциальным временны́м затуханием.
    /// Если история отсутствует — возвращает популярные товары.
    /// </summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="count">Количество рекомендуемых позиций (по умолчанию 8).</param>
    Task<List<RecommendationDto>> GetRecommendationsAsync(string sessionId, int count = 8);

    /// <summary>Возвращает товары, похожие на указанный, по категории, материалу и ценовому диапазону.</summary>
    /// <param name="chetkasId">Идентификатор исходного товара.</param>
    /// <param name="count">Количество похожих позиций (по умолчанию 6).</param>
    Task<List<RecommendationDto>> GetSimilarAsync(int chetkasId, int count = 6);

    /// <summary>
    /// Фиксирует просмотр товара в рамках сессии. Повторный просмотр одного товара
    /// в течение 30 минут игнорируется.
    /// </summary>
    /// <param name="sessionId">Идентификатор сессии покупателя.</param>
    /// <param name="chetkasId">Идентификатор просматриваемого товара.</param>
    Task RecordViewAsync(string sessionId, int chetkasId);
}
