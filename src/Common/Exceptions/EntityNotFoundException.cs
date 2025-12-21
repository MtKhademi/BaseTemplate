namespace Infrastructure.Exceptions;

public class EntityNotFoundException : NotFoundException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with id '{key}' was not found.", "EntityNotFound")
    {
    }
}

public class EntityNotFoundException<TEntity, TKey> : EntityNotFoundException
    where TEntity : class
{
    public EntityNotFoundException(TKey id)
        : base(typeof(TEntity).Name, id!)
    {
    }
}
