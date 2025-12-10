namespace NotificationModule.Services;

[DIScope]
internal class SmsNotificationModuleSender : INotificationModuleSender
{
    public NotificationType Type => NotificationType.Sms;

    public Task SendAsync(string destination, string content, CancellationToken ctx)
    {
        throw new NotImplementedException();
    }
}
