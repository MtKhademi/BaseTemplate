namespace Infrastructure.DDD;

public class BaseEntity
{
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}

public class Entity<TId> : BaseEntity, IEntity<TId>
{
    public TId Id { get; set; }
}
