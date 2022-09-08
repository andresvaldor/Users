namespace Users.Domain.Contracts;

public interface IRepository<T, TKey> where T : Entity<TKey>
{
    Task<T?> Add(T entity);

    Task<T?> Update(T entity);

    Task<bool> Delete(T entity);

    Task<T?> Get(TKey id);
}