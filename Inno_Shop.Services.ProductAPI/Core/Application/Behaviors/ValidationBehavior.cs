using FluentValidation;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : 
	IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

	public async Task<TResponse> Handle(
		TRequest request, 
		RequestHandlerDelegate<TResponse> next, 
		CancellationToken cancellationToken)
	{
		if (!_validators.Any())
			return await next();

		var context = new ValidationContext<TRequest>(request);
		var errorsDictionary = _validators
		.Select(x => x.Validate(context))
		.SelectMany(x => x.Errors)
		.Where(x => x != null)
		.GroupBy(
			x => x.PropertyName[(x.PropertyName.IndexOf('.') + 1)..],
			x => x.ErrorMessage,
			(propertyName, errorMessages) => new
			{
				Key = propertyName,
				Values = errorMessages.Distinct().ToArray() 
			})
		.ToDictionary(x => x.Key, x => x.Values);
		if (errorsDictionary.Count > 0)
			throw new ValidationAppException(errorsDictionary);
		return await next();
	}
}
