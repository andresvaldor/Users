using Users.Domain.Contracts;
using Users.Domain.Exceptions;

namespace Users.Domain.Aggregates.User;

public class User : Entity<Guid>, IAggregateRoot
{
    public string Username { get; private set; }
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public string Email { get; private set; }

    public User(string username, string firstname, string lastname, string email)
    {
        Username = username ?? throw new UsernameNotProvidedException();
        Firstname = firstname ?? throw new FirstnameNotProvidedException();
        Lastname = lastname ?? throw new LastnameNotProvidedException();
        Email = email ?? throw new EmailNotProvidedException();
    }

    public User UpdateUser(string? username, string? firstname, string? lastname, string? email)
    {
        if (!string.IsNullOrEmpty(username))
        {
            Username = username;
        }

        if (!string.IsNullOrEmpty(firstname))
        {
            Firstname = firstname;
        }

        if (!string.IsNullOrEmpty(lastname))
        {
            Lastname = lastname;
        }

        if (!string.IsNullOrEmpty(email))
        {
            Email = email;
        }

        return this;
    }
}