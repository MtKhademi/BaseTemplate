namespace NotificationModule.Contract.Models;

public record NotificationModel
{

    public NotificationResponse ToResponse()
        => new NotificationResponse();

}
