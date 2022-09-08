using Users.Domain.Contracts;

namespace Users.Domain.Exceptions;

public class UsernameNotProvidedException : DomainException<ApplicationError>
{
    public override ApplicationError ErrorCode => ApplicationError.UsernameNotProvided;

    public override string Template => "The username must be provided";
}