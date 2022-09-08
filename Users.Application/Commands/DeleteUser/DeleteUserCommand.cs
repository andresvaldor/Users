using MediatR;

namespace Users.Application.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Unit>
{
    public DeleteUserDto DeleteUserDto { get; set; }

    public DeleteUserCommand(DeleteUserDto deleteUserDto)
    {
        DeleteUserDto = deleteUserDto;
    }
}