using FluentValidation;
using MediatR;

namespace Users.Application.Behaviors.Validation;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = validators.Select(v => v.Validate(context)).SelectMany(x => x.Errors).Where(x => x != null).ToList();

        return failures.Any() ? throw new ValidationException(failures) : next();
    }
}