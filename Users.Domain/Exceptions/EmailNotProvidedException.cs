using Users.Domain.Contracts;

namespace Users.Domain.Exceptions;

public class EmailNotProvidedException : DomainException<ApplicationError>
{
    public override ApplicationError ErrorCode => ApplicationError.EmailNotProvided;

    public override string Template => "The user email must be provided";
}