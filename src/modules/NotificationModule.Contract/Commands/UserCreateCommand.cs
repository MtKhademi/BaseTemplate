namespace NotificationModule.Contract.Commands;

public record SmsNotificationCommand : ICommand<NotificationModel>
{
    public string PhoneNumber { get; init; }
    public string Message { get; init; }

    public SmsNotificationCommand(
        string phoneNumber,
        string message)
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(phoneNumber))
            errors.Add("Phone number is required.");
        if (string.IsNullOrWhiteSpace(message))
            errors.Add("Message is required.");

        if (errors.Any())
            throw new SmsNotificationCommandException(errors);

        PhoneNumber = phoneNumber!;
        Message = message!;
    }


    public static SmsNotificationCommand Create(SmsNotificationRequest request)
        => new SmsNotificationCommand(
           phoneNumber: request.PhoneNumber ?? "",
           message: request.Message);

    internal class SmsNotificationCommandException : NotValidDataException<SmsNotificationCommandException>
    {
        public SmsNotificationCommandException(IEnumerable<string> errors) :
            base(errors)
        {
        }
    }
}