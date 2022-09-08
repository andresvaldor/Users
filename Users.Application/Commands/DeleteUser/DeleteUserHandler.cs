using MediatR;
using Users.Application.Queries.GetUserByUsername;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Application.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository userRepository;
    private readonly IMediator mediator;

    public DeleteUserHandler(IUserRepository userRepository, IMediator mediator)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserByUsernameQuery(new GetUserByUsernameDto() { Username = request.DeleteUserDto.Username }));

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await userRepository.Delete(user);

        return Unit.Value;
    }
}