using Microsoft.OpenApi.Models;
using Users.API.Options;

namespace Users.API.Installers;

public static class SwaggerServicesInstaller
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Users", Version = "v1" });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerApp(this IApplicationBuilder builder, IConfiguration configuration)
    {
        var swaggerOptions = configuration.GetSection(nameof(SwaggerOptions)).Get<SwaggerOptions>();

        builder.UseSwagger(options =>
        {
            options.RouteTemplate = swaggerOptions.JsonRoute;
        });

        builder.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
        });

        return builder;
    }
}