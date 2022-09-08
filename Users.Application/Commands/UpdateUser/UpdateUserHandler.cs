using MediatR;
using Users.Application.Queries.GetUserById;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Application.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository repository;
    private readonly IMediator mediator;

    public UpdateUserHandler(IUserRepository repository, IMediator mediator)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserByIdQuery(new GetUserByIdDto() { Id = request.UpdateUserDto.Id }), cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var updateRequest = request.UpdateUserDto;

        var toUpdateUser = user.UpdateUser(updateRequest.Username, updateRequest.Firstname, updateRequest.Lastname, updateRequest.Email);

        await repository.Update(toUpdateUser);

        return Unit.Value;
    }
}