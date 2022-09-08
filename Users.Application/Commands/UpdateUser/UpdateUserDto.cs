namespace Users.Application.Commands.UpdateUser;

public class UpdateUserDto
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Email { get; set; }
}