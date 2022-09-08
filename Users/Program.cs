using Users.API.Installers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersioning();
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();

if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Services.AddDatabase(builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerApp(builder.Configuration);
app.UseRouting();
app.MapControllers();
app.UseCustomServices();

app.Run();

public partial class Program
{ }