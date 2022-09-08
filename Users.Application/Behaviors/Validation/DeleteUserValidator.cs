using FluentValidation;
using Users.Application.Commands.DeleteUser;
using Users.Domain.Exceptions;

namespace Users.Application.Behaviors.Validation;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.DeleteUserDto.Username).NotEmpty().WithState(x => throw new UsernameNotProvidedException());
    }
}