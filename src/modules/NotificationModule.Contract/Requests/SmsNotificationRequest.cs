namespace NotificationModule.Contract.Requests;

public record SmsNotificationRequest(string PhoneNumber, string Message);
