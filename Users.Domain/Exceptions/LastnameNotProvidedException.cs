using Users.Domain.Contracts;

namespace Users.Domain.Exceptions;

public class LastnameNotProvidedException : DomainException<ApplicationError>
{
    public override ApplicationError ErrorCode => ApplicationError.LastnameNotProvided;

    public override string Template => "The user last name must be provided";
}