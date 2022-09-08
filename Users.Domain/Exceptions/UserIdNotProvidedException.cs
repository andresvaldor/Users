using Users.Domain.Contracts;

namespace Users.Domain.Exceptions;

public class UserIdNotProvidedException : DomainException<ApplicationError>
{
    public override ApplicationError ErrorCode => ApplicationError.UserIdNotProvided;

    public override string Template => "The user Id must be provided";
}