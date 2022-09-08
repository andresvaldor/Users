namespace Users.API.Contracts.v1.Requests;

public class UpdateUserRequest
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Email { get; set; }
}