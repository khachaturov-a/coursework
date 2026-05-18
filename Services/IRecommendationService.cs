using Practos3.Models;

namespace Practos3.Services;

public interface IRecommendationService
{
    Task<List<RecommendationDto>> GetRecommendationsAsync(string sessionId, int count = 8);
    Task<List<RecommendationDto>> GetSimilarAsync(int chetkasId, int count = 6);
    Task                          RecordViewAsync(string sessionId, int chetkasId);
}
