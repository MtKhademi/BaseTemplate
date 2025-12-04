namespace Infrastructure.DateTimeHelpers.DateTimeProviders;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}
