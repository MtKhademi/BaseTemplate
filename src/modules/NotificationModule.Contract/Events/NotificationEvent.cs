namespace NotificationModule.Contract.Events;

public record NotificationModuleEvent : IntegrationEvent
{
    public NotificationType Type { get; set; }
    public int MyProperty { get; set; }
}
