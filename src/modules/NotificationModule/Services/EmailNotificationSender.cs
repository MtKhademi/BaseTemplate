namespace NotificationModule.Services;

[DIScope]
internal class EmailNotificationModuleSender : INotificationModuleSender
{
    public NotificationType Type => NotificationType.Email;

    public Task SendAsync(string destination, string content, CancellationToken ctx)
    {
        throw new NotImplementedException();
    }
}
