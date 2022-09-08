using System.Net;
using Users.Domain.Exceptions;

namespace Users.API.Contracts.v1;

public static class ErrorStatusCodes
{
    public static IDictionary<HttpStatusCode, ApplicationError[]> ErrorDictionary =>
        new Dictionary<HttpStatusCode, ApplicationError[]>
        {
            {
                    HttpStatusCode.BadRequest, new ApplicationError[]
                    {
                        ApplicationError.UsernameNotProvided,
                        ApplicationError.FirstnameNotProvided,
                        ApplicationError.LastnameNotProvided,
                        ApplicationError.EmailNotProvided,
                        ApplicationError.UserIdNotProvided
                    }
                    },
            {
                    HttpStatusCode.NotFound, new ApplicationError[]
                    {
                        ApplicationError.UserNotFound
                    }
            }
        };
}