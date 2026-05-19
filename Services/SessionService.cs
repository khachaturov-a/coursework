namespace Coursework.Services;

/// <summary>
/// Scoped-сервис для хранения идентификатора сессии покупателя в рамках одного
/// Blazor-подключения. Инициализируется из <c>localStorage</c> в MainLayout.
/// </summary>
public class SessionService
{
    private string _sessionId = string.Empty;

    /// <summary>Идентификатор текущей сессии покупателя.</summary>
    public string SessionId => _sessionId;

    /// <summary>Возвращает <c>true</c>, если сессия уже инициализирована.</summary>
    public bool IsInitialized => !string.IsNullOrEmpty(_sessionId);

    /// <summary>
    /// Срабатывает один раз после того, как сессия инициализирована.
    /// Страницы подписываются на это событие, чтобы начать загрузку данных
    /// даже при прямом обновлении страницы (F5), когда сессия ещё не готова
    /// в момент первого рендера.
    /// </summary>
    public event Action? SessionReady;

    /// <summary>Устанавливает идентификатор сессии и уведомляет подписчиков.</summary>
    /// <param name="sessionId">Идентификатор из <c>localStorage</c> или новый GUID.</param>
    public void Initialize(string sessionId)
    {
        _sessionId = sessionId;
        SessionReady?.Invoke();
    }
}
