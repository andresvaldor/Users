using AutoMapper;
using Users.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.Data.Models;

namespace Users.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserContext context;
    private readonly IMapper mapper;

    public UserRepository(UserContext context, IMapper mapper)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<User?> Add(User entity)
    {
        var user = mapper.Map<UserDataModel>(entity);

        user.Id = Guid.NewGuid();

        context.Add(user);

        var insertedRecords = await context.SaveChangesAsync();

        if (insertedRecords == 1)
        {
            return mapper.Map<User>(user);
        }

        return null;
    }

    public async Task<bool> Delete(User entity)
    {
        var user = new UserDataModel()
        {
            Id = entity.Id
        };

        context.Users.Remove(user);

        var deletedRecords = await context.SaveChangesAsync();

        return deletedRecords == 1;
    }

    public async Task<User?> Get(Guid id)
    {
        return mapper.Map<User>(await context.Users.FindAsync(id));
    }

    public async Task<User?> GetByUsername(string username)
    {
        return mapper.Map<User>(await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username));
    }

    public async Task<User?> Update(User entity)
    {
        var user = context.Users.SingleOrDefault(x => x.Id == entity.Id);

        if (user is not null)
        {
            user.Username = entity.Username;
            user.Firstname = entity.Firstname;
            user.Lastname = entity.Lastname;
            user.Email = entity.Email;

            await context.SaveChangesAsync();

            return mapper.Map<User>(user);
        }

        return null;
    }
}