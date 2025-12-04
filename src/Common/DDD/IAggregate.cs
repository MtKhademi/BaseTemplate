namespace Infrastructure.DDD;

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}

public interface IAggregate<TId> : IAggregate, IEntity<TId>
{
}
