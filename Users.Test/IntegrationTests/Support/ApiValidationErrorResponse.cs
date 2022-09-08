namespace Users.Test.IntegrationTests.Support;

public class ApiValidationErrorResponse
{
    public int Status { get; set; }

    public dynamic? Errors { get; set; }
}