using MediatR;
using Users.Domain.Aggregates.User;

namespace Users.Application.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, User?>
{
    private readonly IUserRepository userRepository;

    public CreateUserHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<User?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createUserRequest = request.CreateUserDto;

        User user = new(createUserRequest.Username, createUserRequest.Firstname, createUserRequest.Lastname, createUserRequest.Email);

        return await userRepository.Add(user);
    }
}