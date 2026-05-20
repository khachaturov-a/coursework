namespace Coursework.Services;

/// <summary>
/// Scoped-сервис для оповещения MainLayout об изменении корзины или избранного.
/// Все компоненты одного Blazor-подключения разделяют один экземпляр.
/// </summary>
public class CounterNotifier
{
    public event Func<Task>? OnCountersChanged;

    public async Task NotifyAsync()
    {
        if (OnCountersChanged is null) return;

        foreach (var handler in OnCountersChanged.GetInvocationList().Cast<Func<Task>>())
            await handler();
    }
}
