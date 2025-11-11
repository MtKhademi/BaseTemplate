namespace Common.Hangfier;

public interface IRecurringJob
{
    string Name { get; }
    string CronExpression { get; }
    Task ExecuteAsync();
}
