using MediatR;
using Users.Domain.Aggregates.User;

namespace Users.Application.Commands.CreateUser;

public class CreateUserCommand : IRequest<User>
{
    public CreateUserDto CreateUserDto { get; set; }

    public CreateUserCommand(CreateUserDto createUserDto)
    {
        CreateUserDto = createUserDto;
    }
}