using Users.Domain.Contracts;

namespace Users.Domain.Aggregates.User;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByUsername(string username);
}