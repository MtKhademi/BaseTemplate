namespace Infrastructure.Hangfier;

public interface IRecurringJob
{
    string Name { get; }
    string CronExpression { get; }
    Task ExecuteAsync();
}
