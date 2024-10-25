using Inno_Shop.Services.ProductAPI.Core.Domain.ErrorModel;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Inno_Shop.Services.ProductAPI.Presentation.Extensions;

public static class ExceptionMiddlewareExtensions
{
	public static void ConfigureExceptionHandler(this WebApplication app)
	{
		app.UseExceptionHandler(appError =>
		{
			appError.Run(async context =>
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";
				var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
				if (contextFeature != null)
				{
					context.Response.StatusCode = contextFeature.Error switch
					{
						NotFoundException => StatusCodes.Status404NotFound,
						_ => StatusCodes.Status500InternalServerError
					};

					await context.Response.WriteAsync(new ErrorDetails()
					{
						StatusCode = context.Response.StatusCode,
						Message = contextFeature.Error.Message,
					}.ToString());
				}
			});
		});
	}
}