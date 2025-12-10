namespace NotificationModule.Services;

internal interface INotificationModuleSender
{
    NotificationType Type { get; }
    bool CanHandle(NotificationType type) => Type == type;
    Task SendAsync(string destination, string content, CancellationToken ctx);
}
