using MediatR;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Application.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IUserRepository userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.Get(request.GetUserDto.Id);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }
}