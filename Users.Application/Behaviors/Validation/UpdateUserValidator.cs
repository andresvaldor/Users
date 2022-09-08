using FluentValidation;
using Users.Application.Commands.UpdateUser;
using Users.Domain.Exceptions;

namespace Users.Application.Behaviors.Validation;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.UpdateUserDto.Id).NotEmpty().WithState(x => throw new UserIdNotProvidedException());
        RuleFor(x => x.UpdateUserDto.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.UpdateUserDto.Email)).WithState(x => throw new EmailNotProvidedException());
    }
}