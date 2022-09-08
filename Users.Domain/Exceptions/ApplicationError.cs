namespace Users.Domain.Exceptions;

public enum ApplicationError
{
    UsernameNotProvided = 1000,
    UserNotFound = 1001,
    FirstnameNotProvided = 1002,
    LastnameNotProvided = 1003,
    EmailNotProvided = 1004,
    UserIdNotProvided = 1005
}