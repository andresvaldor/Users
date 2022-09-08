using MediatR;
using FluentValidation.AspNetCore;
using Users.Application.Behaviors.Validation;
using Users.API.Middleware;
using Users.API.Contracts.v1;
using Users.Domain.Exceptions;
using Users.Domain.Aggregates.User;
using Users.Infrastructure.Data.Repositories;
using Users.API.Options;
using Users.Infrastructure.Data;
using Users.Application.Queries.GetUserById;
using Microsoft.EntityFrameworkCore;

namespace Users.API.Installers;

public static class CustomServicesInstaller
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));

        services.AddMediatR(typeof(GetUserByIdHandler).Assembly);
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetUserQueryValidator>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var dataOptions = configuration.GetSection(nameof(DataOptions)).Get<DataOptions>();

        services.AddTransient(p => new UserContext(dataOptions.ConnectionString));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
    {
        services.AddDbContext<UserContext>(options => options.UseInMemoryDatabase("TestingDB"));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IApplicationBuilder UseCustomServices(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandler<ApplicationError>>(ErrorStatusCodes.ErrorDictionary);
        return builder;
    }
}