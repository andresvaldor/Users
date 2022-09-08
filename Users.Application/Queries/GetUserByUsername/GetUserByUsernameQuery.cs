using MediatR;
using Users.Domain.Aggregates.User;

namespace Users.Application.Queries.GetUserByUsername;

public class GetUserByUsernameQuery : IRequest<User?>
{
    public GetUserByUsernameDto GetUserDto { get; set; }

    public GetUserByUsernameQuery(GetUserByUsernameDto getUserDto)
    {
        GetUserDto = getUserDto;
    }
}