using MediatR;
using Users.Domain.Aggregates.User;

namespace Users.Application.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<User?>
{
    public GetUserByIdDto GetUserDto { get; set; }

    public GetUserByIdQuery(GetUserByIdDto getUserDto)
    {
        GetUserDto = getUserDto;
    }
}