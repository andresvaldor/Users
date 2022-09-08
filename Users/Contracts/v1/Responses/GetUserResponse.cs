namespace Users.API.Contracts.v1.Responses
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}