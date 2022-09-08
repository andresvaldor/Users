namespace Users.Domain.Contracts;

public abstract class Entity<TKey>
{
    public TKey? Id { get; protected set; } = default;
}