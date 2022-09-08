using MediatR;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Application.Queries.GetUserByUsername;

public class GetUserByUserNameHandler : IRequestHandler<GetUserByUsernameQuery, User>
{
    private readonly IUserRepository userRepository;

    public GetUserByUserNameHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<User> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByUsername(request.GetUserDto.Username);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }
}