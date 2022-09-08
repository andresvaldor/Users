using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Users.Domain.Aggregates.User;
using Users.Infrastructure.Data;
using Users.Infrastructure.Data.Repositories;

namespace Users.Test.IntegrationTests.Support;

public class UsersWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    private readonly string ConnectionString;

    public UsersWebApplicationFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("https_port", "5001").UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<UserContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<UserContext>(options =>
            {
                options.UseInMemoryDatabase(ConnectionString);
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserContext>();

                db.Database.EnsureCreated();
            }

            services.AddScoped<IUserRepository, UserRepository>();
        });
    }
}