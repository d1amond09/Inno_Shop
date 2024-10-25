using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;

namespace Inno_Shop.Services.ProductAPI.Presentation.Extensions;

public static class ApiBaseResponseExtensions
{
	public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse) => 
		((ApiOkResponse<TResultType>)apiBaseResponse).Result;

}
