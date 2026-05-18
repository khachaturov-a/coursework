namespace Practos3.Services;

public class SessionService
{
    private string _sessionId = string.Empty;

    public string SessionId => _sessionId;
    public bool IsInitialized => !string.IsNullOrEmpty(_sessionId);

    public void Initialize(string sessionId) => _sessionId = sessionId;
}
