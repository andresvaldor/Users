using Users.Domain.Contracts;

namespace Users.Domain.Exceptions;

public class FirstnameNotProvidedException : DomainException<ApplicationError>
{
    public override ApplicationError ErrorCode => ApplicationError.FirstnameNotProvided;

    public override string Template => "The user first name must be provided";
}