using System.Net;
using System.Text.Json;
using Users.Domain.Contracts;

namespace Users.API.Middleware;

public class ExceptionHandler<TErrorCodeEnum> where TErrorCodeEnum : Enum
{
    private readonly RequestDelegate next;
    private readonly IDictionary<HttpStatusCode, TErrorCodeEnum[]> errorDictionary;

    public ExceptionHandler(RequestDelegate next, IDictionary<HttpStatusCode, TErrorCodeEnum[]> errorDictionary)
    {
        this.next = next;
        this.errorDictionary = errorDictionary ?? throw new ArgumentNullException(nameof(errorDictionary));
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/text";

        var domainException = exception as DomainException<TErrorCodeEnum>;
        if (domainException is not null)
        {
            HttpStatusCode? code = errorDictionary.Keys.FirstOrDefault(k => errorDictionary[k].Contains(domainException.ErrorCode));
            context.Response.StatusCode = (int)(code ?? HttpStatusCode.InternalServerError);
            return context.Response.WriteAsync(JsonSerializer.Serialize(new { Code = domainException.ErrorCode, Message = domainException.Message }), System.Text.Encoding.UTF8);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new { Code = (int)HttpStatusCode.InternalServerError, Message = exception.InnerException?.Message ?? exception.Message }), System.Text.Encoding.UTF8);
        }
    }
}