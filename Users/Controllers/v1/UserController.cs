using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.API.Contracts.v1;
using Users.API.Contracts.v1.Requests;
using Users.API.Contracts.v1.Responses;
using Users.Application.Commands.CreateUser;
using Users.Application.Commands.DeleteUser;
using Users.Application.Commands.UpdateUser;
using Users.Application.Queries.GetUserByUsername;

namespace Users.Controllers;

[ApiVersion("1", Deprecated = false)]
[ApiController]
[Route(Routes.Vehicles.Controller)]
public class UserController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<IActionResult> GetUser([FromQuery] GetUserRequest getUserRequest)
    {
        var user = await mediator.Send(new GetUserByUsernameQuery(mapper.Map<GetUserByUsernameDto>(getUserRequest)));

        var response = mapper.Map<GetUserResponse>(user);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
    {
        await mediator.Send(new CreateUserCommand(mapper.Map<CreateUserDto>(createUserRequest)));

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUserRequest)
    {
        await mediator.Send(new UpdateUserCommand(mapper.Map<UpdateUserDto>(updateUserRequest)));

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest deleteUserRequest)
    {
        await mediator.Send(new DeleteUserCommand(mapper.Map<DeleteUserDto>(deleteUserRequest)));

        return Ok();
    }
}