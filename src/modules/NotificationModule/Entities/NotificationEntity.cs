namespace NotificationModule.Entities;

public class NotificationEntity
{
    public Guid Id { get; private set; }

    public Guid? UserId { get; private set; }
    public NotificationType Type { get; private set; }

    public string Destination { get; private set; } 
    public string Payload { get; private set; }     

    public DateTime RequestedAtUtc { get; private set; }
    public DateTime? SentAtUtc { get; private set; }

    public NotificationStatus Status { get; private set; }

    //=> ------- provider info ------- <=//
    public string Provider { get; private set; }
    public string ProviderRequest { get; private set; }   
    public string ProviderResponse { get; private set; } 
    public int? ProviderStatusCode { get; private set; }

    public string ErrorMessage { get; private set; }

    private NotificationEntity() { }

    public NotificationEntity(
        Guid? userId,
        NotificationType type,
        string destination,
        string payload)
    {
        Id = Guid.NewGuid();

        UserId = userId;
        Type = type;
        Destination = destination;
        Payload = payload;

        RequestedAtUtc = DateTime.UtcNow;
        Status = NotificationStatus.Pending;
    }

    public void MarkSuccess(
        string provider,
        string request,
        string response,
        int? statusCode = null)
    {
        Provider = provider;
        ProviderRequest = request;
        ProviderResponse = response;
        ProviderStatusCode = statusCode;

        SentAtUtc = DateTime.UtcNow;
        Status = NotificationStatus.Sent;
    }

    public void MarkFailed(
        string provider,
        string request,
        string error,
        int? statusCode = null)
    {
        Provider = provider;
        ProviderRequest = request;
        ErrorMessage = error;
        ProviderStatusCode = statusCode;

        SentAtUtc = DateTime.UtcNow;
        Status = NotificationStatus.Failed;
    }
}
