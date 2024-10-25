using Inno_Shop.Services.ProductAPI.Core.Domain.ErrorModel;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.ProductAPI.Presentation.Controllers;

public class ApiControllerBase : ControllerBase
{
	public IActionResult ProcessError(ApiBaseResponse baseResponse)
	{
		return baseResponse switch
		{
			ApiNotFoundResponse => NotFound(new ErrorDetails
			{
				Message = ((ApiNotFoundResponse)baseResponse).Message,
				StatusCode = StatusCodes.Status404NotFound
			}),
			ApiBadRequestResponse => BadRequest(new ErrorDetails
			{
				Message = ((ApiBadRequestResponse)baseResponse).Message,
				StatusCode = StatusCodes.Status400BadRequest
			}),
			_ => throw new NotImplementedException()
		};
	}
}

