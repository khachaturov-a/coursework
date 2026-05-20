namespace Coursework.Services;

/// <summary>
/// Scoped-сервис для хранения идентификатора сессии покупателя в рамках одного
/// Blazor-подключения. Инициализируется из <c>localStorage</c> в MainLayout.
/// </summary>
public class SessionService
{
    private string _sessionId = string.Empty;

    public string SessionId    => _sessionId;
    public bool   IsInitialized => !string.IsNullOrEmpty(_sessionId);

    /// <summary>
    /// Срабатывает один раз после инициализации сессии. Страницы подписываются на это
    /// событие, чтобы начать загрузку при прямом обновлении страницы (F5), когда сессия
    /// ещё не готова в момент первого рендера.
    /// </summary>
    public event Action? SessionReady;

    public void Initialize(string sessionId)
    {
        _sessionId = sessionId;
        SessionReady?.Invoke();
    }
}
