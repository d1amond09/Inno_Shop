using Inno_Shop.Services.UserAPI.Core.Domain.Responses;

namespace Inno_Shop.Services.UserAPI.Presentation.Extensions;

public static class ApiBaseResponseExtensions
{
    public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse) =>
        ((ApiOkResponse<TResultType>)apiBaseResponse).Result;

}
