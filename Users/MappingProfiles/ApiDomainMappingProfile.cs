using AutoMapper;
using Users.API.Contracts.v1.Requests;
using Users.API.Contracts.v1.Responses;
using Users.Application.Commands.CreateUser;
using Users.Application.Commands.DeleteUser;
using Users.Application.Commands.UpdateUser;
using Users.Application.Queries.GetUserByUsername;
using Users.Domain.Aggregates.User;
using Users.Infrastructure.Data.Models;

namespace Users.API.MappingProfiles;

public class ApiDomainMappingProfile : Profile
{
    public ApiDomainMappingProfile()
    {
        CreateMap<GetUserRequest, GetUserByUsernameDto>();
        CreateMap<User, GetUserResponse>();

        CreateMap<CreateUserRequest, CreateUserDto>();

        CreateMap<UpdateUserRequest, UpdateUserDto>();

        CreateMap<DeleteUserRequest, DeleteUserDto>();

        CreateMap<UserDataModel, User>();
        CreateMap<User, UserDataModel>();
    }
}