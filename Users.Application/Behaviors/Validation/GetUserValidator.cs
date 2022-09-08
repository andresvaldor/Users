using FluentValidation;
using Users.Application.Queries.GetUserByUsername;
using Users.Domain.Exceptions;

namespace Users.Application.Behaviors.Validation;

public class GetUserQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.GetUserDto.Username).NotEmpty().WithState(x => throw new UsernameNotProvidedException());
    }
}