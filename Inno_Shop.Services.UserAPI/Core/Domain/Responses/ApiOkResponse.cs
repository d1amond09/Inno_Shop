namespace Inno_Shop.Services.UserAPI.Core.Domain.Responses;

public sealed class ApiOkResponse<TResult>(TResult result) : 
	ApiBaseResponse(true)
{
	public TResult Result { get; set; } = result;
}

