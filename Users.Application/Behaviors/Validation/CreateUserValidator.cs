using FluentValidation;
using Users.Application.Commands.CreateUser;
using Users.Application.Commands.DeleteUser;
using Users.Domain.Exceptions;

namespace Users.Application.Behaviors.Validation;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.CreateUserDto.Username).NotEmpty().WithState(x => throw new UsernameNotProvidedException());
        RuleFor(x => x.CreateUserDto.Firstname).NotEmpty().WithState(x => throw new FirstnameNotProvidedException());
        RuleFor(x => x.CreateUserDto.Lastname).NotEmpty().WithState(x => throw new LastnameNotProvidedException());
        RuleFor(x => x.CreateUserDto.Email).NotEmpty().WithState(x => throw new EmailNotProvidedException());
        RuleFor(x => x.CreateUserDto.Email).EmailAddress().WithState(x => throw new EmailNotProvidedException());
    }
}