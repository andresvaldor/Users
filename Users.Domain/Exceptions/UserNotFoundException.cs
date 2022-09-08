using Users.Domain.Contracts;

namespace Users.Domain.Exceptions;

public class UserNotFoundException : DomainException<ApplicationError>
{
    public override ApplicationError ErrorCode => ApplicationError.UserNotFound;

    public override string Template => "User not found in the system";
}