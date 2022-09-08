using MediatR;

namespace Users.Application.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Unit>
{
    public UpdateUserDto UpdateUserDto { get; set; }

    public UpdateUserCommand(UpdateUserDto updateUserDto)
    {
        UpdateUserDto = updateUserDto;
    }
}