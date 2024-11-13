using System.Net;
using System.Text.Json;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.ProductAPI.Presentation.GlobalException;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
	private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext, 
		Exception exception, 
		CancellationToken cancellationToken)
	{
		httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		httpContext.Response.ContentType = "application/json";
		var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

		httpContext.Response.StatusCode = contextFeature?.Error switch
		{
			NotFoundException => StatusCodes.Status404NotFound,
			BadRequestException => StatusCodes.Status400BadRequest,
			ValidationAppException => StatusCodes.Status422UnprocessableEntity,
			_ => StatusCodes.Status500InternalServerError
		};
		
		if (contextFeature?.Error is ValidationAppException ex)
		{
			var errorMessages = ex.Errors.Select(error =>
			{
				var key = error.Key;
				var values = error.Value;
				var valuesStr = string.Join(", ", values);
				return $"{key}: {valuesStr}";
			});

			return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
			{
				HttpContext = httpContext,
				ProblemDetails =
				{
					Title = ex.Message,
					Detail = string.Join(" ", errorMessages),
					Type = ex.GetType().Name,
					Status = httpContext.Response.StatusCode
				},
				Exception = exception,
			});
		}

		return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
		{
			HttpContext = httpContext,
			ProblemDetails =
			{
				Title = "An error occured",
				Detail = exception.Message,
				Type = exception.GetType().Name,
				Status = httpContext.Response.StatusCode
			},
			Exception = exception,
		});
	
	}
}
